using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechF.Configuration
{
    //ncrunch: no coverage start
    public static class ConfigurationManager
    {
        public static IClassFactoryConfiguration Copy(this ClassFactorySection config)
        {
            if (config == null)
                return null;

            return new ClassFactoryConfiguration()
            {
                ClassFactories
                    = from c in config.ClassFactories ?? new ClassFactoryCollection()
                      select new ClassFactory()
                      {
                          Type = c.Type,
                          PersistPerContext = c.PersistsPerContext,
                          FactoryType = c.FactoryType,
                          FactoryProperties = from p in c.FactoryProperties ?? new ClassFactoryPropertyCollection()
                                              select new ClassFactoryProperty()
                                              {
                                                  Name = p.Name,
                                                  Value = p.Value
                                              }}};
        }
        public static IClassFactoryConfiguration ClassFactoryConfigurationFromConfigurationFile(string section)
        {
            return ((ClassFactorySection)System.Configuration.ConfigurationManager.GetSection(section)).Copy();
        }
    }
    //ncrunch: no coverage end
}
