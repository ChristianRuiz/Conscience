using Conscience.Application.Graph.Entities.Accounts;
using Conscience.Domain;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Application.Graph.Entities.Locations
{
    public class LocationGraphType : ConscienceGraphType<Location>
    {
        public LocationGraphType()
        {
            Name = "Location";
            
            Field(l => l.Latitude);
            Field(l => l.Longitude);
            Field(l => l.TimeStamp);
            Field<FloatGraphType>("accuracy", resolve: context => context.Source.Accuracy);
            Field<FloatGraphType>("altitude", resolve: context => context.Source.Altitude);
            Field<FloatGraphType>("altitudeAccuracy", resolve: context => context.Source.AltitudeAccuracy);
            Field<FloatGraphType>("heading", resolve: context => context.Source.Heading);
            Field<FloatGraphType>("headingAccuracy", resolve: context => context.Source.HeadingAccuracy);
            Field<FloatGraphType>("speed", resolve: context => context.Source.Speed);
        }
    }
}
