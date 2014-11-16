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
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechF.Example.BasicUsage
{
    public interface IDummyDataSource
    {
        string Text { get; }
    }
    public interface IDummyService
    {
        void Speak();
    }
    public class DummyDataSource : IDummyDataSource
    {
        // This property will be set automatically using property binding.
        public string Text { get; set; }
    }
    public class DummyService : IDummyService
    {
        private IDummyDataSource _dataSource;

        public DummyService(IDummyDataSource dataSource)
        {
            // The class factory generated code will automatically
            // resolve reference types (besides string) using the class
            // factory context. Value types will be resolved by parameter name
            // in the property object provided on type configuration
            _dataSource = dataSource;
        }
        
        public void Speak()
        {
            Console.WriteLine(_dataSource.Text);
        }
    }


    // This is the main class for the application.
    // By keeping a class which references all the major components,
    // it is possible for the class factory to construct all depenncies
    // and types automatically without the components needing to know 
    // about the class factory. This lets you reuse your entire application
    // without being dependent on the class factory
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
            // Enter Class Factory context
            using(var context = new ClassFactoryContext())
            {
                // Create property dictionary to use for
                // setting constructor argument and applying
                // property binding
                var props = ImmutableDictionary<string, string>.Empty.Add("Text", "Hello world!");
                // Initialize IDummyDataSource to use the DummyDataSource
                // implementation using the provided properties for property
                // binding the Text property (properties are bound when they
                // have a public set accesor)
                context.SetFactoryType<IDummyDataSource, DummyDataSource>(props);
                // Initialize IDummyService to use the DummyService
                // implementation. The constructor argument dataSource
                // is resolved using the class factory context on constructor 
                // call time
                context.SetFactoryType<IDummyService, DummyService>();
                // Initialize the main class Application. Application
                // should have all needed services required as constructor
                // arguments, so the class factory can initialize the entire
                // application without any need to call the class factory
                // later on in the application. This allows complete decoupling
                // of the class factory component from the rest of the application
                context.SetFactoryType<Application, Application>();
                
                // Resolve the application object from the class factory
                var application = context.Resolve<Application>();
                // Run the application
                application.Run();
            }
        }
    }
}
