using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechF.Configuration
{
    public interface IClassFactoryProperty
    {
        string Name { get; }
        string Value { get; }
    }
    public interface IClassFactory
    {
        string Type { get; }
        string FactoryType { get; }
        bool PersistPerContext { get; }
        IEnumerable<IClassFactoryProperty> FactoryProperties { get; }
    }
    public interface IClassFactoryConfiguration
    {
        IEnumerable<IClassFactory> ClassFactories { get; }
    }
}
