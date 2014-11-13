using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechF.Exceptions
{
    public class ValidationError : Exception
    {
        //ncrunch: no coverage start
        private string[] _errors;

        public ValidationError(IEnumerable<string> errors)
        {
            _errors = errors.ToArray();
        }

        public override string ToString()
        {
            return string.Join("\n", _errors);
        }
        //ncrunch: no coverage end
    }
}
