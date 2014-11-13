using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechF.Utilities
{
    public class DictionaryComparison<TKey, TValue> where TValue : class
    {
        public static bool Compare(IReadOnlyDictionary<TKey, TValue> x, IReadOnlyDictionary<TKey, TValue> y)
        {
            //
            if (null == y)
                return null == x;
            if (null == x)
                return false;
            if (object.ReferenceEquals(x, y))
                return true;
            if (x.Count != y.Count)
                return false;
            foreach (TKey k in x.Keys)
                if (!y.ContainsKey(k))
                    return false;

            //
            foreach (TKey k in x.Keys)
            {
                var xk = x != null ? x[k] : null;
                var yk = y != null ? y[k] : null;

                if (xk == null && yk != null)
                    return false;

                if (xk != yk)
                    return false;
            }

            return true;
        }
    }
}
