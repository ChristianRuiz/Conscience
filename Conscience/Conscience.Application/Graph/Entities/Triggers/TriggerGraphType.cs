using Conscience.Domain;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Application.Graph.Entities.Triggers
{
    public class TriggerGraphType : ObjectGraphType<Trigger>
    {
        public TriggerGraphType()
        {
            Name = "Trigger";

            Field(t => t.Id);
            Field(t => t.Description);
        }
    }
}
