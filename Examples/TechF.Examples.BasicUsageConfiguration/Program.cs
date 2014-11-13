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
