using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechF.Examples.BasicWebApi
{
    public interface IDummyService
    {
        IEnumerable<Models.DummyData> GetData();
    }
}
