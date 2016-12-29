using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Conscience.Domain.Enums;

namespace Conscience.Plugins.Droid
{
    public class BatteryService : IBatteryService
    {
        public int RemainingChargePercent
        {
            get
            {
                try
                {
                    using (var filter = new IntentFilter(Intent.ActionBatteryChanged))
                    {
                        using (var battery = Application.Context.RegisterReceiver(null, filter))
                        {
                            var level = battery.GetIntExtra(Android.OS.BatteryManager.ExtraLevel, -1);
                            var scale = battery.GetIntExtra(Android.OS.BatteryManager.ExtraScale, -1);

                            return (int)Math.Floor(level * 100D / scale);
                        }
                    }
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("Ensure you have android.permission.BATTERY_STATS");
                    throw;
                }

            }
        }

        public BatteryStatus Status
        {
            get
            {
                try
                {
                    using (var filter = new IntentFilter(Intent.ActionBatteryChanged))
                    {
                        using (var battery = Application.Context.RegisterReceiver(null, filter))
                        {
                            int status = battery.GetIntExtra(Android.OS.BatteryManager.ExtraStatus, -1);
                            var isCharging = status == (int)BatteryStatus.Charging || status == (int)BatteryStatus.Full;

                            var chargePlug = battery.GetIntExtra(Android.OS.BatteryManager.ExtraPlugged, -1);
                            var usbCharge = chargePlug == (int)Android.OS.BatteryPlugged.Usb;
                            var acCharge = chargePlug == (int)Android.OS.BatteryPlugged.Ac;
                            bool wirelessCharge = false;
                            wirelessCharge = chargePlug == (int)Android.OS.BatteryPlugged.Wireless;

                            isCharging = (usbCharge || acCharge || wirelessCharge);
                            if (isCharging)
                                return BatteryStatus.Charging;

                            switch (status)
                            {
                                case (int)BatteryStatus.Charging:
                                    return BatteryStatus.Charging;
                                case (int)BatteryStatus.Discharging:
                                    return BatteryStatus.Discharging;
                                case (int)BatteryStatus.Full:
                                    return BatteryStatus.Full;
                                case (int)BatteryStatus.NotCharging:
                                    return BatteryStatus.NotCharging;
                                default:
                                    return BatteryStatus.Unknown;
                            }
                        }
                    }
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("Ensure you have android.permission.BATTERY_STATS");
                    throw;
                }
            }
        }

        public PowerSource PowerSource
        {
            get
            {
                try
                {
                    using (var filter = new IntentFilter(Intent.ActionBatteryChanged))
                    {
                        using (var battery = Application.Context.RegisterReceiver(null, filter))
                        {
                            int status = battery.GetIntExtra(Android.OS.BatteryManager.ExtraStatus, -1);
                            var isCharging = status == (int)BatteryStatus.Charging || status == (int)BatteryStatus.Full;

                            var chargePlug = battery.GetIntExtra(Android.OS.BatteryManager.ExtraPlugged, -1);
                            var usbCharge = chargePlug == (int)Android.OS.BatteryPlugged.Usb;
                            var acCharge = chargePlug == (int)Android.OS.BatteryPlugged.Ac;

                            bool wirelessCharge = false;
                            wirelessCharge = chargePlug == (int)Android.OS.BatteryPlugged.Wireless;

                            isCharging = (usbCharge || acCharge || wirelessCharge);

                            if (!isCharging)
                                return PowerSource.Battery;
                            else if (usbCharge)
                                return PowerSource.Usb;
                            else if (acCharge)
                                return PowerSource.Ac;
                            else if (wirelessCharge)
                                return PowerSource.Wireless;
                            else
                                return PowerSource.Other;
                        }
                    }
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("Ensure you have android.permission.BATTERY_STATS");
                    throw;
                }
            }
        }
    }
}