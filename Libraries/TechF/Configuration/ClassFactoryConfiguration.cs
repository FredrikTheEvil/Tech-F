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
