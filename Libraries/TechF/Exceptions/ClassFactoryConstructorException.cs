using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechF.Exceptions
{
    public class ClassFactoryConstructorException : ClassFactoryException
    {
        //ncrunch: no coverage start
        public ClassFactoryConstructorException() : base() { }
        public ClassFactoryConstructorException(string msg) : base() { }
        public ClassFactoryConstructorException(string msg, Exception inner) : base() { }
        //ncrunch: no coverage end
    }
}
