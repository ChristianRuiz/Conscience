using Conscience.Application.Graph.Entities.Accounts;
using Conscience.Application.Graph.Entities.Locations;
using Conscience.Domain;
using Conscience.Domain.Enums;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Application.Graph.Entities.Devices
{
    public class DeviceGraphType : ConscienceGraphType<Device>
    {
        public DeviceGraphType()
        {
            Name = "Device";

            Field(d => d.DeviceId);
            Field(d => d.LastConnection);
            Field(d => d.Online);
            Field(d => d.BatteryLevel);
            Field<BatteryStatusEnumGraphType>("batteryStatus", resolve: context => context.Source.BatteryStatus);
            Field<PowerSourceEnumGraphType>("powerSource", resolve: context => context.Source.PowerSource);
            Field<LocationGraphType>("currentLocation", resolve: context => context.Source.CurrentLocation);
            Field<ListGraphType<LocationGraphType>>("locations", resolve: context => context.Source.Locations);
        }
    }

    public class BatteryStatusEnumGraphType : EnumerationGraphType<BatteryStatus>
    {
        public BatteryStatusEnumGraphType()
        {
            Name = "BatteryStatus";
        }
    }

    public class PowerSourceEnumGraphType : EnumerationGraphType<PowerSource>
    {
        public PowerSourceEnumGraphType()
        {
            Name = "PowerSources";
        }
    }
}
