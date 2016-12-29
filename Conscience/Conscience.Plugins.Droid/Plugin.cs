using MvvmCross.Platform.Plugins;
using MvvmCross.Platform;

namespace Conscience.Plugins.Droid
{
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            Mvx.RegisterType<IBatteryService, BatteryService>();
            Mvx.RegisterSingleton<IAudioService>(new AudioService());
        }
    }
}