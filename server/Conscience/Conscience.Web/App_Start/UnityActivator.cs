using System.Linq;
using System.Web.Mvc;
using Microsoft.Practices.Unity.Mvc;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using System.Web.Http;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.WebApi;

[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(Conscience.Web.UnityActivator), "Shutdown")]
namespace Conscience.Web
{
    /// <summary>Provides the bootstrapping for integrating Unity with ASP.NET MVC.</summary>
    public static class UnityActivator
    {
        /// <summary>Integrates Unity when the application starts.</summary>
        public static IUnityContainer Start() 
        {
            var container = UnityConfig.GetConfiguredContainer();
            
            DependencyResolver.SetResolver(new Microsoft.Practices.Unity.Mvc.UnityDependencyResolver(container));
            
            GlobalConfiguration.Configuration.DependencyResolver = new UnityHierarchicalDependencyResolver(container);

            return container;
        }

        /// <summary>Disposes the Unity container when the application is shut down.</summary>
        public static void Shutdown()
        {
            var container = UnityConfig.GetConfiguredContainer();
            container.Dispose();
        }
    }
}