using Microsoft.AspNet.SignalR;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Conscience.Web
{
    public class SignalRDependencyResolver : DefaultDependencyResolver
    {
        protected IUnityContainer _container;

        public SignalRDependencyResolver(IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            this._container = container.CreateChildContainer();
        }

        public override object GetService(Type serviceType)
        {
            try
            {
                return _container.Resolve(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return base.GetService(serviceType);
            }
        }

        public override IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return _container.ResolveAll(serviceType).Concat(base.GetServices(serviceType));
            }
            catch (ResolutionFailedException)
            {
                return base.GetServices(serviceType);
            }
        }

        protected override void Dispose(bool disposing)
        {
            _container.Dispose();
            base.Dispose(disposing);
        }
    }
}