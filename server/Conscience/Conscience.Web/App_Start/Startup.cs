using Microsoft.Practices.Unity;
using Microsoft.Owin;
using Owin;
using Conscience.DataAccess;
using System.Web.Mvc;
using System.Web.Http;
using System.Web.Routing;
using System.Web.Optimization;
using Conscience.Web.Logger;
using System;

[assembly: OwinStartupAttribute(typeof(Conscience.Web.Startup))]
namespace Conscience.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Log4NetLogger.Current.WriteDebug("App Startup");

            try
            {

                AreaRegistration.RegisterAllAreas();
                GlobalConfiguration.Configure(WebApiConfig.Register);
                FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
                RouteConfig.RegisterRoutes(RouteTable.Routes);
                BundleConfig.RegisterBundles(BundleTable.Bundles);

                ConfigureAuth(app);

                var container = UnityActivator.Start();

                app.MapSignalR(new Microsoft.AspNet.SignalR.HubConfiguration
                {
                    Resolver = new SignalRDependencyResolver(container)
                });
                
                var context = container.Resolve<ConscienceContext>();
                context.Database.CreateIfNotExists();
                context.Database.Initialize(true);
            }
            catch (Exception ex)
            {
                Log4NetLogger.Current.WriteError("App Startup Exception", ex);

                throw;
            }
        }
    }
}
