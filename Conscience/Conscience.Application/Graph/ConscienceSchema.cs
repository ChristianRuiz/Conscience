using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Application.Graph
{
    public class ConscienceSchema : Schema
    {
        public ConscienceSchema(Func<Type, GraphType> resolveType)
            : base(resolveType)
        {
            Query = (ConscienceQuery)resolveType(typeof(ConscienceQuery));
            Mutation = (ConscienceMutation)resolveType(typeof(ConscienceMutation));
        }
    }
}
