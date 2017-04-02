using GraphQL.Types;
﻿using GraphQL.Language.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using Conscience.DataAccess;
using System.Web;

namespace Conscience.Application
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

        public static IQueryable<T> AvoidLazyLoad<T>(this IQueryable<T> queryable, ResolveFieldContext<object> context, params Expression<Func<T, object>>[] lazyLoadFields)
        {
            var fieldNames = context.Operation.SelectionSet.GetSelectionFieldNames();

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

        public static List<string> GetSelectionFieldNames(this SelectionSet selectionSet)
        {
            var fields = selectionSet.Selections.OfType<GraphQL.Language.AST.Field>();
            var fieldNames = fields.Select(f => f.Name).ToList();

            foreach (var field in fields)
            {
                var subFieldNames = field.SelectionSet.GetSelectionFieldNames();
                if (subFieldNames.Any())
                    fieldNames.AddRange(subFieldNames);
            }
            
            return fieldNames;
        }
    }
}