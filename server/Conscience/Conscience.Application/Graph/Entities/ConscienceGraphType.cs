using Conscience.Domain;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Application.Graph.Entities
{
    public class ConscienceGraphType<T> : ObjectGraphType<T> where T : IdentityEntity
    {
        public ConscienceGraphType()
        {
            Field<IntGraphType>("id", resolve: context => context.Source.Id);
        }
    }
}
