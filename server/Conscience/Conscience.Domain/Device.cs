using Conscience.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Domain
{
    public class Device
    {
        public Device()
        {
            Locations = new List<Location>();
            BatteryStatus = BatteryStatus.Unknown;
        }

        public int Id
        {
            get;
            set;
        }
        
        public DateTime LastConnection
        {
            get;
            set;
        }

        public string DeviceId
        {
            get;
            set;
        }

        public double BatteryLevel
        {
            get;
            set;
        }

        public BatteryStatus BatteryStatus
        {
            get;
            set;
        }
        
        public virtual List<Location> Locations { get; set; }

        public virtual Location CurrentLocation
        {
            get;
            set;
        }

        public bool Online
        {
            get
            {
                return (DateTime.Now - LastConnection) < TimeSpan.FromMinutes(2);
            }
        }
    }
}
