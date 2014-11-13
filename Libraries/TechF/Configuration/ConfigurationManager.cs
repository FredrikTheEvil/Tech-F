// Tech-F Framework
// (C) Copyright 2014 Fredrik A.Kristiansen<fredrikaxk@gmail.com>
//
// All rights reserved.This program and the accompanying materials
// are made available under the terms of the GNU Lesser General Public License
// (LGPL) version 2.1 which accompanies this distribution, and is available at
// http://www.gnu.org/licenses/lgpl-2.1.html
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the GNU
// Lesser General Public License for more details.
//
// Contributors:
//     Fredrik A. Kristiansen<fredrikaxk@gmail.com>
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
