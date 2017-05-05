using Conscience.Mobile.Hosts.Core.Services;
using Conscience.Mobile.Hosts.Core.ViewModels;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Mobile.Hosts.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes();
            Mvx.ConstructAndRegisterSingleton<AppState, AppState>();
            Mvx.ConstructAndRegisterSingleton<GraphQLService, GraphQLService>();
            Mvx.ConstructAndRegisterSingleton<HostsService, HostsService>();
            Mvx.RegisterSingleton<IMvxAppStart>(new MvxAppStart<LoginViewModel>());
        }
    }
}
