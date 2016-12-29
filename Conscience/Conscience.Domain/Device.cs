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
        }

        public int Id
        {
            get;
            set;
        }

        public bool Online
        {
            get;
            set;
        }

        public DateTime LastConnection
        {
            get;
            set;
        }

        public Guid DeviceId
        {
            get;
            set;
        }

        public int BatteryLevel
        {
            get;
            set;
        }

        public BatteryStatus BatteryStatus
        {
            get;
            set;
        }

        public PowerSource PowerSource
        {
            get;
            set;
        }

        public List<Location> Locations { get; set; }

        public Location CurrentLocation
        {
            get
            {
                if (Locations == null)
                    return null;

                return Locations.LastOrDefault();
            }
        }
    }
}
