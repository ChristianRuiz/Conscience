using System.Linq;
using System.Web.Mvc;
using Microsoft.Practices.Unity.Mvc;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using System.Web.Http;
using Microsoft.Practices.Unity;

//[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Conscience.Web.UnityActivator), "PreStart")]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(Conscience.Web.UnityActivator), "Shutdown")]
namespace Conscience.Web
{
    /// <summary>Provides the bootstrapping for integrating Unity with ASP.NET MVC.</summary>
    public static class UnityActivator
    {
        public static void PreStart()
        {
            DynamicModuleUtility.RegisterModule(typeof(UnityPerRequestHttpModule));
        }

        /// <summary>Integrates Unity when the application starts.</summary>
        public static IUnityContainer Start() 
        {
            var container = UnityConfig.GetConfiguredContainer();

            //FilterProviders.Providers.Remove(FilterProviders.Providers.OfType<FilterAttributeFilterProvider>().First());
            //FilterProviders.Providers.Add(new UnityFilterAttributeFilterProvider(container));

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            
            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);

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