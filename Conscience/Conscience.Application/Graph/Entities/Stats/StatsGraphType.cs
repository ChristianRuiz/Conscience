using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Application.Graph.Entities.Stats
{
    public class StatsGraphType : ConscienceGraphType<Domain.Stats>
    {
        public StatsGraphType()
        {
            Name = "Stats";
            
            Field(s => s.Name);
            Field(s => s.Value);
        }
    }
}
