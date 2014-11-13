using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechF.Exceptions
{
    public class ClassFactoryException : Exception
    {
        //ncrunch: no coverage start
        public ClassFactoryException() : base() {}
        public ClassFactoryException(string msg) : base(msg){}
        public ClassFactoryException(string msg, Exception inner) : base(msg, inner) {}
        //ncrunch: no coverage end
    }
}
