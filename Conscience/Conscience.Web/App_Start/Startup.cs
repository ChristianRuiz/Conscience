using Conscience.Web.App_Start;
using Microsoft.Practices.Unity;
using Microsoft.Owin;
using Owin;
using Concience.DataAccess;
using Conscience.Web.Models;

[assembly: OwinStartupAttribute(typeof(Conscience.Web.Startup))]
namespace Conscience.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();

            var container = UnityConfig.GetConfiguredContainer();
            
            var context = container.Resolve<ConscienceContext>();
            context.Database.CreateIfNotExists();
        }
    }
}
