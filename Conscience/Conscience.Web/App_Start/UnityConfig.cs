using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Conscience.DataAccess;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using GraphQL;
using GraphQL.Http;
using GraphQL.Types;
using Conscience.Application.Graph;
using Conscience.Application.Services;
using Conscience.Web.Identity;
using System.Reflection;
using Conscience.DataAccess.Repositories;
using System.Linq;

namespace Conscience.Web
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        private static object _syncLock = new object();
        private static IUnityContainer _container;

        #region Unity Container
        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            lock (_syncLock)
            {
                if (_container == null)
                {
                    _container = CreateConfiguredContainer();
                }
            }

            return _container;
        }

        protected static IUnityContainer CreateConfiguredContainer()
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        protected static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterInstance<IDocumentExecuter>(new DocumentExecuter());
            container.RegisterInstance<IDocumentWriter>(new DocumentWriter(true));
            
            container.RegisterType<ConscienceContext, ConscienceContext>(new HierarchicalLifetimeManager());
            container.RegisterType<IUsersIdentityService, UsersIdentityService>(new HierarchicalLifetimeManager());
        }
    }
}
