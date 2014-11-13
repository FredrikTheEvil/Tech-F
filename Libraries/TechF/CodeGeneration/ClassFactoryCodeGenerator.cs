using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TechF.CodeGeneration
{
    public static class ClassFactoryCodeGenerator
    {
        internal class TypeCodeCacheEntry
        {
            public Type Type { get; set; }
            public ConstructorCall ConstructorCall { get; set; }
            public PropertyBinder PropertyBinder { get; set; }
            public IReadOnlyDictionary<string, string> Properties { get; set; }
            public int FetchCount { get; set; }
        }
        private static Dictionary<Type, IList<TypeCodeCacheEntry>> _typeCodeCache = new Dictionary<Type, IList<TypeCodeCacheEntry>>();
        private static ReaderWriterLock _typeCodeCacheLock = new ReaderWriterLock();

        internal static TypeCodeCacheEntry GetCachedItem(Type type, IReadOnlyDictionary<string, string> props)
        {
            IList<TypeCodeCacheEntry> list = new List<TypeCodeCacheEntry>();
            _typeCodeCacheLock.AcquireReaderLock(-1);
            if(_typeCodeCache.ContainsKey(type))
                list = _typeCodeCache[type];
            _typeCodeCacheLock.ReleaseReaderLock();

            var item = (from c in list where Utilities.DictionaryComparison<string, string>.Compare(c.Properties, props) select c).FirstOrDefault();
            if(item == null)
                item =  Cache(type, props);
            return item;
        }
        internal static TypeCodeCacheEntry Cache(Type type, IReadOnlyDictionary<string, string> props)
        {
            var ctorCall = CreateConstructorInvokeMethod(type, props);
            var propBinder = CreatePropertyBinder(type, props);

            return CacheItem(type, props, ctorCall, propBinder);
        }
        private static TypeCodeCacheEntry CacheItem(Type type, IReadOnlyDictionary<string, string> props, ConstructorCall ctorCall, PropertyBinder propBinder)
        {
            _typeCodeCacheLock.AcquireWriterLock(-1);
            IList<TypeCodeCacheEntry> list = null;
            if(_typeCodeCache.ContainsKey(type))
            {
                list = _typeCodeCache[type];
            } else
            {
                list = new List<TypeCodeCacheEntry>();
                _typeCodeCache.Add(type, list);
            }
            var entry = new TypeCodeCacheEntry()
            {
                Type = type,
                ConstructorCall = ctorCall,
                PropertyBinder = propBinder,
                Properties = props,
                FetchCount = 0
            };
            list.Add(entry);
            _typeCodeCacheLock.ReleaseWriterLock();

            return entry;
        }

        public delegate void PropertyBinder(object obj);
        public delegate object ConstructorCall(ClassFactoryContext context);

        private static void NullBinder(object obj) { }

        private static void EmitDateTime(ILGenerator il, string value)
        {
            var method = typeof(DateTime).GetMethod("Parse", new Type[] { typeof(string) });
            il.Emit(OpCodes.Ldstr, value);
            il.Emit(OpCodes.Call, method);
        }
        private static void EmitDateTimeOffset(ILGenerator il, string value)
        {
            var method = typeof(DateTimeOffset).GetMethod("Parse", new Type[] { typeof(string) });
            il.Emit(OpCodes.Ldstr, value);
            il.Emit(OpCodes.Call, method);
        }
        private static void EmitEnum(ILGenerator il, Type type, string value)
        {
            object enumObj = Enum.Parse(type, value, true);
            int i = (int)enumObj;
            il.Emit(OpCodes.Ldc_I4, i);
        }
        private static void EmitDecimal(ILGenerator il, string value)
        {
            var method = typeof(Decimal).GetMethod("Parse", new Type[] { typeof(string) });
            il.Emit(OpCodes.Ldstr, value);
            il.Emit(OpCodes.Call, method);
        }

        private static bool EmitPropertyValue(ILGenerator il, Type type, string value)
        {
            if (type == typeof(string))
                il.Emit(OpCodes.Ldstr, value);
            else if (type == typeof(byte) || type == typeof(ushort) || type == typeof(uint))
                il.Emit(OpCodes.Ldc_I4, Convert.ToUInt32(value));
            else if (type == typeof(sbyte) || type == typeof(short) || type == typeof(int))
                il.Emit(OpCodes.Ldc_I4, Convert.ToInt32(value));
            else if (type == typeof(long))
                il.Emit(OpCodes.Ldc_I8, Convert.ToInt64(value));
            else if (type == typeof(ulong))
                il.Emit(OpCodes.Ldc_I8, unchecked((long)Convert.ToUInt64(value)));
            else if (type == typeof(bool))
                il.Emit(OpCodes.Ldc_I4, Convert.ToBoolean(value) ? 1 : 0);
            else if (type == typeof(float))
                il.Emit(OpCodes.Ldc_R4, Convert.ToSingle(value));
            else if (type == typeof(double))
                il.Emit(OpCodes.Ldc_R8, Convert.ToDouble(value));
            else if (type == typeof(decimal))
                EmitDecimal(il, value);
            else if (type == typeof(DateTime))
                EmitDateTime(il, value);
            else if (type == typeof(DateTimeOffset))
                EmitDateTimeOffset(il, value);
            else if (type.IsEnum)
                //
                EmitEnum(il, type, value);
            return true;
        }

        public static PropertyBinder CreatePropertyBinder(Type type, IReadOnlyDictionary<string, string> props)
        {
            props = props ?? new Dictionary<string, string>();
            if (props.Count < 1)
                return NullBinder;
            var properties = type.GetProperties().Where(x => x.SetMethod.IsPublic).ToArray();
            if (properties.Length < 1)
                return NullBinder;

            var method = new DynamicMethod("", null, new Type[] { typeof(object) });
            var il = method.GetILGenerator();

            var obj = il.DeclareLocal(type);
            var propertyLocals = new LocalBuilder[properties.Length];
            for (int i = 0; i < properties.Length; i++)
                propertyLocals[i] = il.DeclareLocal(properties[i].PropertyType);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Castclass, type);
            il.Emit(OpCodes.Stloc, obj);



            for (int i = 0; i < properties.Length; i++)
            {
                var property = properties[i];
                if (!(property.PropertyType == typeof(string) || property.PropertyType.IsValueType))
                    continue;
                if (props.ContainsKey(property.Name))
                {
                    if (EmitPropertyValue(il, property.PropertyType, props[property.Name]))
                    {
                        il.Emit(OpCodes.Stloc, propertyLocals[i]);
                        il.Emit(OpCodes.Ldloc, obj);
                        il.Emit(OpCodes.Ldloc, propertyLocals[i]);
                        il.Emit(OpCodes.Call, property.SetMethod);
                    }
                }
            }
            il.Emit(OpCodes.Ret);

            return (PropertyBinder)method.CreateDelegate(typeof(PropertyBinder));
        }

        public static ConstructorCall CreateConstructorInvokeMethod(Type type, IReadOnlyDictionary<string, string> props)
        {
            var method = new DynamicMethod("", typeof(object), new Type[] { typeof(ClassFactoryContext) });
            var il = method.GetILGenerator();
            foreach (var ctor in type.GetConstructors())
            {
                if (!ctor.IsPublic) continue;
                bool resolvedValueParameters = true;
                foreach (var param in ctor.GetParameters())
                    if ((param.ParameterType.IsValueType || param.ParameterType == typeof(string)) && !props.ContainsKey(param.Name))
                    {
                        resolvedValueParameters = false;
                        break;
                    }
                if (resolvedValueParameters)
                {
                    // This constructor is usable
                    // Generate IL code
                    MethodInfo contextResolveMethod = typeof(ClassFactoryContext).GetMethods().Where(x => x.IsPublic && x.Name == "Resolve" && x.ContainsGenericParameters).Single();
                    MethodInfo globalMethod = typeof(ClassFactory).GetMethods().Where(x => x.Name == "Resolve" && x.ContainsGenericParameters).Single();

                    var parameters = ctor.GetParameters();
                    var paramLocals = new LocalBuilder[parameters.Length];

                    for (int i = 0; i < parameters.Length; i++)
                        paramLocals[i] = il.DeclareLocal(parameters[i].ParameterType);

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        var param = parameters[i];
                        if (param.ParameterType.IsValueType || param.ParameterType == typeof(string))
                        {
                            EmitPropertyValue(il, param.ParameterType, props[param.Name]);
                            il.Emit(OpCodes.Stloc, paramLocals[i]);
                        }
                        else
                        {
                            var labelWithContext = il.DefineLabel();
                            var labelAfterContext = il.DefineLabel();

                            il.Emit(OpCodes.Ldarg_0);
                            il.Emit(OpCodes.Brtrue, labelWithContext);
                            // Without class factory
                            var genericMethod = globalMethod.MakeGenericMethod(new Type[] { param.ParameterType });
                            il.Emit(OpCodes.Call, genericMethod);
                            il.Emit(OpCodes.Stloc, paramLocals[i]);
                            il.Emit(OpCodes.Br, labelAfterContext);
                            il.MarkLabel(labelWithContext);
                            // With class factory context
                            var genericMethodContext = contextResolveMethod.MakeGenericMethod(new Type[] { param.ParameterType });
                            il.Emit(OpCodes.Ldarg_0);
                            il.Emit(OpCodes.Call, genericMethodContext);
                            il.Emit(OpCodes.Stloc, paramLocals[i]);

                            il.MarkLabel(labelAfterContext);
                        }
                    }

                    for (int i = 0; i < parameters.Length; i++)
                        il.Emit(OpCodes.Ldloc, paramLocals[i]);
                    il.Emit(OpCodes.Newobj, ctor);
                    il.Emit(OpCodes.Ret);

                    return (ConstructorCall)method.CreateDelegate(typeof(ConstructorCall));
                }

            }
            throw new Exceptions.ClassFactoryConstructorException("No supported constructors are available");
        }
    }
}
