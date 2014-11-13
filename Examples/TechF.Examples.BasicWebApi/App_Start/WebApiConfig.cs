using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace TechF.Examples.BasicWebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Load class factory configuration from web.config
            // Allows web.config to be the entry point for the
            // application, and making it easy to deploy variations
            // of the applications
            var classFactoryConfig = TechF.Configuration.ConfigurationManager.ClassFactoryConfigurationFromConfigurationFile("ClassFactory");
            ClassFactory.LoadFromConfiguration(classFactoryConfig);
            config.DependencyResolver = new DependencyResolver();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
