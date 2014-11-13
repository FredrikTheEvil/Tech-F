using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechF.Exceptions
{
    public class ClassFactoryTypeMismatchException : ClassFactoryException
    {
        //ncrunch: no coverage start
        public ClassFactoryTypeMismatchException() : base() { }
        public ClassFactoryTypeMismatchException(string msg) : base(msg){ }
        public ClassFactoryTypeMismatchException(string msg, Exception inner) : base(msg, inner) { }
        //ncrunch: no coverage end
    }
}
