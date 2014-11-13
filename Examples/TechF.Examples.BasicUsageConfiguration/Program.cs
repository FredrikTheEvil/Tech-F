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

namespace TechF.Examples.BasicUsageConfiguration
{
    public interface IDummyDataSource
    {
        string Text { get; set; }
    }
    public interface IDummyService
    {
        void Speak();
    }
    public class DummyDataSource : IDummyDataSource
    {
        public string Text { get; set; }
    }
    public class DummyService : IDummyService
    {
        private IDummyDataSource _dataSource;

        public DummyService(IDummyDataSource dataSource)
        {
            _dataSource = dataSource;
        }

        public void Speak()
        {
            Console.WriteLine(_dataSource.Text);
        }
    }

    public class Application
    {
        private IDummyService _service;

        public Application(IDummyService service)
        {
            _service = service;
        }

        public void Run()
        {
            _service.Speak();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public static void Main(string[] args)
        {
            var config = TechF.Configuration.ConfigurationManager.ClassFactoryConfigurationFromConfigurationFile("ClassFactory");
            ClassFactory.LoadFromConfiguration(config);
            using (var context = new ClassFactoryContext())
            {
                var application = context.Resolve<Application>();
                application.Run();
            }
        }
    }
}
