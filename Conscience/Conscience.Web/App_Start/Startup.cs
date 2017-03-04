using Microsoft.Practices.Unity;
using Microsoft.Owin;
using Owin;
using Concience.DataAccess;
using Conscience.Web.Models;
using System.Web.Mvc;
using System.Web.Http;
using System.Web.Routing;
using System.Web.Optimization;

[assembly: OwinStartupAttribute(typeof(Conscience.Web.Startup))]
namespace Conscience.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ConfigureAuth(app);
            app.MapSignalR();

            var container = UnityActivator.Start();
            
            var context = container.Resolve<ConscienceContext>();
            context.Database.CreateIfNotExists();
            context.Database.Initialize(false);
        }
    }
}
