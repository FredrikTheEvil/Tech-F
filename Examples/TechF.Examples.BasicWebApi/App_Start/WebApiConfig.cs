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
