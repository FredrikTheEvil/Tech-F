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