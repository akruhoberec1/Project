using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using AnimalTest.App_Start;

namespace AnimalTest.App_Start
{
    public class Bootstrapper
    {
        public static void Run()
        {
            AutofacConfig.Initialize(GlobalConfiguration.Configuration);
        }
    }
}