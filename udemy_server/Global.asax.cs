using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using udemy_server.Controllers;
using udemy_server.Models.Entities;

namespace udemy_server
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private BackgroundTaskManager backgroundTaskManager;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            var udemyController = new UdemyController();

           
            backgroundTaskManager = new BackgroundTaskManager(udemyController);

            
        }
    }
}
