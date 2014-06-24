using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace CustomHelper
{
    internal static class QueryExtension
    {

        public static List<T> Get<T>(IEnumerable<T> list, string propertyName, bool asc, int from, int to)
        {
            if (list != null)
            {
                IQueryable<T> query = list.AsQueryable<T>();
                if (string.IsNullOrEmpty(propertyName))
                    return query.Take(to).ToList<T>();

                IQueryable<T> tmpList = QueryExtension.OrderBy(query, propertyName, asc).Skip(from).Take(to);
                return tmpList.ToList<T>();
            }
            return null;
        }

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, string memberName, bool asc)
        {
            ParameterExpression[] typeParams = new ParameterExpression[] { Expression.Parameter(typeof(T), "") };
            System.Reflection.PropertyInfo pi = typeof(T).GetProperty(memberName);

            return (IOrderedQueryable<T>)query.Provider.CreateQuery(
                Expression.Call(
                    typeof(Queryable),
                    asc ? "OrderBy" : "OrderByDescending",
                    new Type[] { typeof(T), pi.PropertyType },
                    query.Expression,
                    Expression.Lambda(Expression.Property(typeParams[0], pi), typeParams))
            );
        }
    }
}
