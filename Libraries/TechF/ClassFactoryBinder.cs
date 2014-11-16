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
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TechF.CodeGeneration;

namespace TechF
{
    public class ClassFactoryOptions
    {
        internal ClassFactoryOptions(ClassFactoryBinder binder)
        {
            _binder = new WeakReference<ClassFactoryBinder>(binder);
        }
        private WeakReference<ClassFactoryBinder> _binder;

        public bool Persist
        {
            set
            {
                ClassFactoryBinder binder = null;
                if (_binder.TryGetTarget(out binder))
                {
                    binder._optionsLock.AcquireWriterLock(-1);
                    binder._persist = value;
                    binder._optionsLock.ReleaseWriterLock();
                }
            }
        }
        public bool BindProperties
        {
            set
            {
                ClassFactoryBinder binder = null;
                if (_binder.TryGetTarget(out binder))
                {
                    binder._optionsLock.AcquireWriterLock(-1);
                    binder._bindProperties = value;
                    binder._optionsLock.ReleaseWriterLock();
                }
            }
        }
    }
    public class ClassFactoryBinder
    {
        internal ClassFactoryBinder(ClassFactoryContext context, ClassFactoryBinder binder)
        {
            _context = context;
            _type = binder._type;
            _ctorCall = binder._ctorCall;
            _properties = binder._properties;
            _options = new ClassFactoryOptions(this);
            _persist = binder._persist;
            _lastBuild = new WeakReference(null);
        }
        internal ClassFactoryBinder(ClassFactoryContext context, Type type, ImmutableDictionary<string, string> properties)
        {
            _context = context;
            _type = type;
            _properties = properties;
            _options = new ClassFactoryOptions(this);
            _lastBuild = new WeakReference(null);
        }

        internal ClassFactoryContext _context;
        internal Type _type;
        internal Func<ClassFactoryBinder, object> _build;
        internal WeakReference _lastBuild = null;

        internal ImmutableDictionary<string, string> _properties;

        internal bool _persist;
        internal bool _bindProperties = true;
        internal ClassFactoryOptions _options;
        internal ReaderWriterLock _optionsLock = new ReaderWriterLock();

        internal ClassFactoryCodeGenerator.ConstructorCall _ctorCall = null;
        internal ClassFactoryCodeGenerator.PropertyBinder _propBinder = null;

        internal void Bind(object obj)
        {
            if (_propBinder == null)
            {
                var cachedCode = ClassFactoryCodeGenerator.GetCachedItem(_type, _properties);
                _propBinder = cachedCode.PropertyBinder;
            }

            _propBinder(obj);
        }

        internal object Create()
        {
            if (_ctorCall == null)
                _ctorCall = ClassFactoryCodeGenerator.GetCachedItem(_type, _properties).ConstructorCall;

            var persist = false;
            var propertyBind = false;
            _optionsLock.AcquireReaderLock(-1);
            persist = _persist;
            propertyBind = _bindProperties;
            _optionsLock.ReleaseReaderLock();

            if (_context == null)
            {
                // Do not persist in global scope
                var o = _ctorCall(null);
                if (propertyBind)
                    Bind(o);
                return _ctorCall(null);
            }
            var lastBuild = _lastBuild.Target;
            if (lastBuild != null && _lastBuild.IsAlive && persist)
                return lastBuild;

            lastBuild = _ctorCall(_context);
            if (propertyBind)
                Bind(lastBuild);
            _lastBuild.Target = lastBuild;

            return lastBuild;
        }
    }
}
