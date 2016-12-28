using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Conscience.Plugins
{
    public interface IBatteryService
    {
        int RemainingChargePercent { get; }
        BatteryStatus Status { get; }
        PowerSource PowerSource { get; }
    }

    public enum BatteryStatus
    {
        Charging,
        Discharging,
        Full,
        NotCharging,
        Unknown
    }

    public enum PowerSource
    {
        Battery,
        Ac,
        Usb,
        Wireless,
        Other
    }
}
