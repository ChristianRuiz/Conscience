using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System.Linq.Dynamic
{
    public static class GraphQLQueriableExtensions
    {
        public static IQueryable<T> ApplyPaginationAndOrderBy<T>(this IQueryable<T> queryable, ResolveFieldContext<object> context)
        {
            var result = queryable;

            if (context.Arguments["orderby"] != null)
                result = result.OrderBy(context.GetArgument<string>("orderby"));
            else
                result = result.OrderBy("Id");

            if (context.Arguments["offset"] != null)
                result = result.Skip(context.GetArgument<int>("offset"));

            if (context.Arguments["first"] != null)
                result = result.Take(context.GetArgument<int>("first"));

            return result;
        }
    }
}