// Tech-F Framework
// (C) Copyright 2014 Fredrik A.Kristiansen<fredrikaxk@gmail.com>
//
// All rights reserved.This program and the accompanying materials
// are made available under the terms of the GNU Lesser General Public License
// (LGPL) version 2.1 which accompanies this distribution, and is available at
// http://www.gnu.org/licenses/lgpl-2.1.html
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the GNU
// Lesser General Public License for more details.
//
// Contributors:
//     Fredrik A. Kristiansen<fredrikaxk@gmail.com>
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TechF.Configuration;

namespace TechF
{
    /// <summary>
    /// Global Class Factory singleton. Configures global factories and properties
    /// </summary>
    public class ClassFactory
    {
        //ncrunch: no coverage start
        internal static Dictionary<System.Type, ClassFactoryBinder> _factories = new Dictionary<Type, ClassFactoryBinder>();
        internal static ReaderWriterLock _factoriesLock = new ReaderWriterLock();
        //ncrunch: no coverage end

        private static ClassFactoryBinder ResolveBinder(Type type)
        {
            _factoriesLock.AcquireReaderLock(-1);
            var binder = _factories.ContainsKey(type) ? _factories[type] : null;
            _factoriesLock.ReleaseReaderLock();
            return binder;
        }
        /// <summary>
        /// Resolve a object for the specified type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object Resolve(Type type)
        {
            var binder = ResolveBinder(type);
            if(binder != null)
                return binder.Create();
            throw new Exceptions.ClassFactoryTypeMismatchException();
        }
        /// <summary>
        /// Resolve a object for the specified type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Resolve<T>() where T : class
        {
            return (T)Resolve(typeof(T));
        }
        /// <summary>
        /// Remove the configuration for the specified type
        /// </summary>
        /// <param name="type"></param>
        public static void ClearFactoryTypeInformation(Type type)
        {
            _factoriesLock.AcquireWriterLock(-1);
            if (_factories.ContainsKey(type))
                _factories.Remove(type);
            _factoriesLock.ReleaseWriterLock();
        }
        /// <summary>
        /// Remove the configuration for the specified type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void ClearFactoryTypeInformation<T>()
        {
            ClearFactoryTypeInformation(typeof(T));
        }
        /// <summary>
        /// Configure a factory for the given type using the provided 
        /// constant values used in constructor injection and property binding
        /// </summary>
        /// <param name="type">The type to configure</param>
        /// <param name="factoryType">The type that implements 'type'</param>
        /// <param name="properties">Constant properties for parameter binding and constructor injection</param>
        /// <returns>A options object that can be used for setting persistence and property binding</returns>
        public static ClassFactoryOptions SetFactoryType(Type type, Type factoryType, IReadOnlyDictionary<string, string> properties = null)
        {
            if (!type.IsAssignableFrom(factoryType))
                throw new Exceptions.ClassFactoryTypeMismatchException();
            var binder = new ClassFactoryBinder(null, factoryType, properties);

            _factoriesLock.AcquireWriterLock(-1);
            _factories[type] = binder;
            _factoriesLock.ReleaseWriterLock();

            return binder._options;
        }
        /// <summary>
        /// Configure a factory for the given type using the provided 
        /// constant values used in constructor injection and property binding
        /// </summary>
        /// <typeparam name="TType">The type to configure</typeparam>
        /// <typeparam name="TImplementation">The type that implements 'type'</typeparam>
        /// <param name="properties">Constant properties for parameter binding and constructor injection</param>
        /// <returns>A options object that can be used for setting persistence and property binding</returns>
        public static ClassFactoryOptions SetFactoryType<TType, TImplementation>(IReadOnlyDictionary<string, string> properties = null) where TType : class where TImplementation : TType
        {
            return SetFactoryType(typeof(TType), typeof(TImplementation), properties);
        }
        //ncrunch: no coverage start
        /// <summary>
        ///  Load class factory configuration
        /// </summary>
        /// <param name="configuration"></param>
        public static void LoadFromConfiguration(IClassFactoryConfiguration configuration)
        {
            foreach (var factory in configuration.ClassFactories)
            {
                var typeName = factory.Type;
                var factoryTypeName = factory.FactoryType;

                var props = factory.FactoryProperties ?? new IClassFactoryProperty[] { };
                IReadOnlyDictionary<string, string> factoryBinder = props.ToDictionary(x => x.Name, x => x.Value);

                try
                {
                    Type type = Type.GetType(typeName);
                    Type factoryType = Type.GetType(factoryTypeName);


                    if (type != null &&  factoryType != null && type.IsAssignableFrom(factoryType))
                    {
                        SetFactoryType(type, factoryType, factoryBinder).Persist = factory.PersistPerContext;
                    }
                }
                catch
                {
                }
            }
        }
        //ncrunch: no coverage end
    }
}
