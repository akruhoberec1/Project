using Autofac;
using AnimalTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AnimalTest.App_Start
{
    public class AutofacConfig
    {
        //public static void ConfigureContainer()
        //{
        //    var builder = new ContainerBuilder();

        //    // Register dependencies in controllers
        //    builder.RegisterControllers(typeof(WebApiApplication).Assembly);

        //    // Register dependencies in filter attributes
        //    builder.RegisterFilterProvider();

        //    // Register dependencies in custom views
        //    builder.RegisterSource(new ViewRegistrationSource());

        //    // Register our Data dependencies
        //    builder.RegisterModule(new DataModule("MVCWithAutofacDB"));

        //    var container = builder.Build();

        //    // Set MVC DI resolver to use our Autofac container
        //    DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        //}
    }
}