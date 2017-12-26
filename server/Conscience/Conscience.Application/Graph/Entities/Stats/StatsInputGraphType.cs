using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Application.Graph.Entities.Stats
{
    public class StatsInputGraphType : ConscienceInputGraphType
    {
        public StatsInputGraphType() : base()
        {
            Name = "StatsInput";

            Field<StringGraphType>("name");
            Field<IntGraphType>("value");
        }
    }
}
