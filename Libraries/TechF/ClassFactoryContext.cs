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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TechF
{
    public class ClassFactoryContext : IDisposable
    {
        private Dictionary<System.Type, ClassFactoryBinder> _factories = new Dictionary<Type, ClassFactoryBinder>();
        internal ReaderWriterLock _factoriesLock = new ReaderWriterLock();

        private ClassFactoryBinder ResolveRoot(Type type)
        {
            ClassFactory._factoriesLock.AcquireReaderLock(-1);
            var binder = ClassFactory._factories.ContainsKey(type) ? ClassFactory._factories[type] : null;
            ClassFactory._factoriesLock.ReleaseReaderLock();
            return binder;
        }
        private ClassFactoryBinder ResolveContext(Type type)
        {
            _factoriesLock.AcquireReaderLock(-1);
            var binder = _factories.ContainsKey(type) ? _factories[type] : null;
            _factoriesLock.ReleaseReaderLock();
            return binder;
        }

        /// <summary>
        /// Resolve a object of the specified type from the context,
        /// falling back to the global class factory if not found in the context.
        /// If resolved from the global class factory, the 
        /// configuration is added to the context
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <throws name="ClassFactoryTypeMismatch">If the type is not configured or valid for creation</throws>
        public object Resolve(Type type)
        {
            ClassFactoryBinder binder = ResolveContext(type);
            if (binder != null)
            {
                var o = binder.Create();
                return o;
            }
            binder = ResolveRoot(type);
            if (binder != null)
            {
                var contextBinder = new ClassFactoryBinder(this, binder);
                _factoriesLock.AcquireWriterLock(-1);
                _factories[binder._type] = contextBinder;
                _factoriesLock.ReleaseWriterLock();
                return contextBinder.Create();
            }
            throw new Exceptions.ClassFactoryException(string.Format("Type '{0}' is not configured", type.Name));
        }
        /// <summary>
        /// Resolve a object of the specified type from the context,
        /// falling back to the global class factory if not found in the context.
        /// If resolved from the global class factory, the 
        /// configuration is added to the context
        /// </summary>
        /// <typeparam name="T">The type to create</typeparam>
        /// <returns>A object of the requested type</returns>
        /// <throws name="ClassFactoryTypeMismatch">If the type is not configured or valid for creation</throws>
        public T Resolve<T>() where T : class
        {
            return (T)Resolve(typeof(T));
        }
        /// <summary>
        /// Configure a context bound factory for the given type using the provided 
        /// constant values used in constructor injection and property binding
        /// </summary>
        /// <param name="type">The type to configure</param>
        /// <param name="factoryType">The type that implements 'type'</param>
        /// <param name="properties">Constant properties for parameter binding and constructor injection</param>
        /// <returns>A options object that can be used for setting persistence and property binding</returns>
        public ClassFactoryOptions SetFactoryType(Type type, Type factoryType, IReadOnlyDictionary<string, string> properties = null)
        {
            if (!type.IsAssignableFrom(factoryType))
                throw new Exceptions.ClassFactoryTypeMismatchException();

            var binder = new ClassFactoryBinder(this, factoryType, properties);

            _factoriesLock.AcquireWriterLock(-1);
            _factories[type] = binder;
            _factoriesLock.ReleaseWriterLock();

            return binder._options;
        }
        /// <summary>
        /// Configure a context bound factory for the given type using the provided 
        /// constant values used in constructor injection and property binding
        /// </summary>
        /// <param name="type">The type to configure</param>
        /// <param name="factoryType">The type that implements 'type'</param>
        /// <param name="properties">Constant properties for parameter binding and constructor injection</param>
        /// <returns>A options object that can be used for setting persistence and property binding</returns>
        public ClassFactoryOptions SetFactoryType<TType, TImplementation>(IReadOnlyDictionary<string, string> properties = null) where TType : class where TImplementation : TType
        {
            return SetFactoryType(typeof(TType), typeof(TImplementation), properties);
        }
        /// <summary>
        /// Get the options object for the specified type, in order to set persistence and property binding 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ClassFactoryOptions GetTypeOptions(Type type)
        {
            ClassFactoryBinder binder = ResolveContext(type);
            if (binder != null)
                return binder._options;
            binder = ResolveRoot(type);
            if (binder != null)
                return binder._options;
            throw new Exceptions.ClassFactoryTypeMismatchException();
        }
        /// <summary>
        /// Get the options object for the specified type, in order to set persistence and property binding 
        /// </summary>
        /// <typeparam name="type"></typeparam>
        /// <returns></returns>
        public ClassFactoryOptions GetTypeOptions<T>()
        {
            return GetTypeOptions(typeof(T));
        }
        /// <summary>
        /// Remove the configuration for the specified type
        /// </summary>
        /// <param name="type"></param>
        public void ClearFactoryTypeInformation(Type type)
        {
            _factoriesLock.AcquireWriterLock(-1);
            if (_factories.ContainsKey(type))
                _factories.Remove(type);
            _factoriesLock.ReleaseWriterLock();
        }
        /// <summary>
        /// Remove the configuration for the specified type
        /// </summary>
        /// <typeparam name="type"></typeparam>
        public void ClearFactoryTypeInformation<T>()
        {
            ClearFactoryTypeInformation(typeof(T));
        }

        //ncrunch: no coverage start
        public void Dispose()
        {
        }
        //ncrunch: no coverage end
    }
}
