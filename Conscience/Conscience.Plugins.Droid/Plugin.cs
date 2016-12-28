using MvvmCross.Platform.Plugins;
using MvvmCross.Platform;

namespace Conscience.Plugins.Android
{
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            Mvx.RegisterType<IBatteryService, BatteryService>();
        }
    }
}