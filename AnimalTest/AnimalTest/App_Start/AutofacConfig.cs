using Autofac;
using AnimalTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac.Integration.WebApi;
using System.Data.Odbc;
using System.Reflection;
using System.Web.Http;
using AnimalTest.Service.Common;
using Microsoft.Ajax.Utilities;
using AnimalTest.Service;
using AnimalTest.Repository;
using AnimalTest.Repository.Common;
using System.Web.Http.Controllers;

namespace AnimalTest.App_Start
{
    public class AutofacConfig
    {
        public static IContainer Container;

        public static void Initialize(HttpConfiguration config)
        {
            Initialize(config, RegisterServices(new ContainerBuilder()));
        }
        public static void Initialize(HttpConfiguration config, IContainer container)
        {
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static IContainer RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<EmployeeService>().As<IEmployeeService>();
            builder.RegisterType<EmployeeRepository>().As<IEmployeeRepository>();

            Container = builder.Build();

            return Container;

        }
    }
}