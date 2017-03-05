
using Conscience.Domain;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Application
{
    public static class LazyLoadExtensions
    {
        //public static IQueryable<Host> AvoidLazyLoad<Host>(this IQueryable<Host> queryable, ResolveFieldContext<object> context)
        //{
        //    return queryable.AvoidLazyLoad(context, e => e.Account, e => e.Device, e => e.Notifications);
        //}
    }
}
