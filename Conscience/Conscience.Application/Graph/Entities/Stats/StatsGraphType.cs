using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Application.Graph.Entities.Stats
{
    public class StatsGraphType : ObjectGraphType<Domain.Stats>
    {
        public StatsGraphType()
        {
            Name = "Stats";

            Field(s => s.Id);
            Field(s => s.Name);
            Field(s => s.Value);
        }
    }
}
