using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Application.Graph.Entities
{
    public class ConscienceGraphType<T> : ObjectGraphType<T>
    {
        public ConscienceGraphType()
        {
            Field<StringGraphType>("id", resolve: context => {
                var id = context.Source.GetType().GetProperty("Id");
                return id.GetValue(context.Source).ToString().ToGraphId();
                });
        }
    }
}
