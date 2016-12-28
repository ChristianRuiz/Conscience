using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Conscience.Web.Startup))]
namespace Conscience.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
