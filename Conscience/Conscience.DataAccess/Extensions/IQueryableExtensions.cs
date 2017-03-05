using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.DataAccess
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> AvoidLazyLoad<T, R>(this IQueryable<T> queryable, Expression<Func<T, R>> relatedObjectSelector)
        {
            queryable = queryable.Include(relatedObjectSelector);

            return queryable;
        }
    }
}
