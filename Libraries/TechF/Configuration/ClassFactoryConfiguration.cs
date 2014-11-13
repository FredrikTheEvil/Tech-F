using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechF.Configuration
{
    //ncrunch: no coverage start
    public class ClassFactoryProperty : IClassFactoryProperty
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
    public class ClassFactory : IClassFactory
    {
        public IEnumerable<IClassFactoryProperty> FactoryProperties { get; set; }

        public string FactoryType { get; set; }
        public string Type { get; set; }

        public bool PersistPerContext { get; set; }
    }
    public class ClassFactoryConfiguration : IClassFactoryConfiguration
    {
        public IEnumerable<IClassFactory> ClassFactories { get; set; }
    }
    //ncrunch: no coverage end
}
