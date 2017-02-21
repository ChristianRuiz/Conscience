using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Application.Graph
{
    public static class ConscienceArguments
    {
        public static QueryArguments IdQueryArgument
        {
            get
            {
                return new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id", Description = "id of the user" }
                );
            }
        }

        public static QueryArguments PaginationsAndSortingArgument
        {
            get
            {
                return new QueryArguments(
                    new QueryArgument<IntGraphType> { Name = "first", Description = "number of items to take" },
                    new QueryArgument<IntGraphType> { Name = "offset", Description = "offset of the first item to return" },
                    new QueryArgument<StringGraphType> { Name = "orderby", Description = "orderby" }
                    );
            }
        }
    }
}
