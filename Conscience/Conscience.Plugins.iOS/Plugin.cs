using MvvmCross.Platform.Plugins;
using MvvmCross.Platform;

namespace Conscience.Plugins.iOS
{
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            Mvx.RegisterType<IBatteryService, BatteryService>();
        }
    }
}