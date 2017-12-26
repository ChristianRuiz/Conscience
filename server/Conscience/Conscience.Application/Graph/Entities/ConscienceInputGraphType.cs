using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Application.Graph.Entities
{
    public class ConscienceInputGraphType : InputObjectGraphType
    {
        public ConscienceInputGraphType()
        {
            Field<IntGraphType>("id");
            Field<StringGraphType>("__typename");
        }
    }
}
