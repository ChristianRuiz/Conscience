using Conscience.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;

namespace Conscience.Plugins.iOS
{
    public class BatteryService : IBatteryService
    {
        public BatteryService()
        {
            UIDevice.CurrentDevice.BatteryMonitoringEnabled = true;
        }

        public int RemainingChargePercent
        {
            get
            {
                return (int)(UIDevice.CurrentDevice.BatteryLevel * 100F);
            }
        }

        public BatteryStatus Status
        {
            get
            {
                switch (UIDevice.CurrentDevice.BatteryState)
                {
                    case UIDeviceBatteryState.Charging:
                        return BatteryStatus.Charging;
                    case UIDeviceBatteryState.Full:
                        return BatteryStatus.Full;
                    case UIDeviceBatteryState.Unplugged:
                        return BatteryStatus.Discharging;
                    default:
                        return BatteryStatus.Unknown;
                }
            }
        }

        public PowerSource PowerSource
        {
            get
            {
                switch (UIDevice.CurrentDevice.BatteryState)
                {
                    case UIDeviceBatteryState.Charging:
                        return PowerSource.Ac;
                    case UIDeviceBatteryState.Full:
                        return PowerSource.Ac;
                    case UIDeviceBatteryState.Unplugged:
                        return PowerSource.Battery;
                    default:
                        return PowerSource.Other;
                }
            }
        }
    }
}