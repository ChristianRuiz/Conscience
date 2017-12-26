using Conscience.Domain;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Application.Graph.Entities.Triggers
{
    public class TriggerInputGraphType : ConscienceInputGraphType
    {
        public TriggerInputGraphType()
        {
            Name = "TriggerInput";

            Field<StringGraphType>("description");
        }
    }
}
