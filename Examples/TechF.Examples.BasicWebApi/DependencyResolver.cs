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
using System.Web;
using System.Web.Http.Dependencies;

namespace TechF.Examples.BasicWebApi
{
    public class DependencyScope : IDependencyScope
    {
        private TechF.ClassFactoryContext _context = new ClassFactoryContext();

        public void Dispose()
        {
            _context.Dispose();
        }

        public object GetService(Type serviceType)
        {
            return _context.Resolve(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return new object[] { _context.Resolve(serviceType) };
        }
    }

    public class DependencyResolver : IDependencyResolver
    {
        private TechF.ClassFactoryContext _context = new TechF.ClassFactoryContext();

        public IDependencyScope BeginScope()
        {
            return new DependencyScope();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return _context.Resolve(serviceType);
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return new object[] { _context.Resolve(serviceType) };
            }
            catch
            {
                return new object[] { };
            }
        }
    }
}