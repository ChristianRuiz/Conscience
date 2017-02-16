using MvvmCross.Platform.Plugins;
using MvvmCross.Platform;
using Foundation;

namespace Conscience.Plugins.iOS
{
	[Preserve(AllMembers = true)]
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            Mvx.RegisterType<IBatteryService, BatteryService>();
			Mvx.RegisterType<IAudioService, AudioService>();
        }
    }
}