using Conscience.Web.Logger;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Conscience.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //Here your SignalR dependency resolver
            GlobalHost.DependencyResolver = new SignalRDependencyResolver(UnityConfig.GetConfiguredContainer());
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Log4NetLogger.Current.WriteError("Unhandled error", HttpContext.Current.Error);
        }
    }
}
