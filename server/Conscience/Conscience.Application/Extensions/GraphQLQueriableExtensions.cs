using GraphQL.Types;
using GraphQL.Language.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using Conscience.DataAccess;
using System.Web;
using Conscience.Domain;

namespace Conscience.Application
{
    public static class GraphQLQueriableExtensions
    {
        public static IQueryable<T> ApplyPaginationAndOrderBy<T>(this IQueryable<T> queryable, ResolveFieldContext<object> context)
            where T : IdentityEntity
        {
            var result = queryable;

            if (context.Arguments.ContainsKey("orderby") && context.Arguments["orderby"] != null)
                result = result.OrderBy(context.GetArgument<string>("orderby"));
            else
                result = result.OrderBy(i => i.Id);

            result = result.ApplyPagination(context);

            return result;
        }

        public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> queryable, ResolveFieldContext<object> context)
            where T : IdentityEntity
        {
            var result = queryable;
            
            if (context.Arguments["offset"] != null)
                result = result.Skip(context.GetArgument<int>("offset"));

            if (context.Arguments["first"] != null)
                result = result.Take(context.GetArgument<int>("first"));

            return result;
        }

        public static IQueryable<T> AvoidLazyLoad<T>(this IQueryable<T> queryable, ResolveFieldContext<object> context, params Expression<Func<T, object>>[] lazyLoadFields)
        {
            var fieldNames = context.GetSelectionFieldNames(context.Operation.SelectionSet);

            foreach (var fieldName in fieldNames)
            {
                var lazyLoadField = lazyLoadFields.FirstOrDefault(f => f.Body is MemberExpression &&
                    ((MemberExpression)f.Body).Member.Name.ToLowerInvariant() == fieldName.ToLowerInvariant());

                if (lazyLoadField != null)
                {
                    queryable = queryable.AvoidLazyLoad(lazyLoadField);
                }
            }

            return queryable;
        }

        public static MemberExpression GetNestedExprProp(Expression expr, string propName)
        {
            var properties = propName.Split('.');
            var propertiesCount = properties.Length;

            if (propertiesCount <= 1) return Expression.Property(expr, propName);

            var lastProperty = properties.Last();
            var propertiesWithoutLast = properties.Take(propertiesCount - 1).Aggregate((a, i) => a + "." + i);

            return Expression.Property(GetNestedExprProp(expr, propertiesWithoutLast), lastProperty);
        }

        public static List<string> GetSelectionFieldNames(this ResolveFieldContext<object> context, SelectionSet selectionSet)
        {
            var fields = selectionSet.Selections.OfType<Field>();
            var fieldNames = fields.Select(f => f.Name).ToList();

            foreach (var field in fields)
            {
                var subFieldNames = context.GetSelectionFieldNames(field.SelectionSet);
                if (subFieldNames.Any())
                    fieldNames.AddRange(subFieldNames);
            }

            var fragments = selectionSet.Selections.OfType<FragmentSpread>();

            foreach(var fragment in fragments)
            {
                var fragmentDefinition = context.Fragments.First(f => f.Name == fragment.Name);
                var subFieldNames = context.GetSelectionFieldNames(fragmentDefinition.SelectionSet);
                if (subFieldNames.Any())
                    fieldNames.AddRange(subFieldNames);
            }

            return fieldNames;
        }
    }
}