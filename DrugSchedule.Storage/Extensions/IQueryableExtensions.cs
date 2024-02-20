using System.Linq;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace DrugSchedule.Storage.Extensions;

public static class IQueryableExtensions
{
    private static Expression<Func<TEntity, bool>> GetStringFilterExpression<TEntity>(Expression<Func<TEntity, string?>> propertySelector, Contract.StringFilter stringFilter)
    {
        Expression<Func<string, bool>> containsExpression = stringFilter.StringSearchType switch
        {
            Contract.StringSearch.StartsWith => x => x.StartsWith(stringFilter.SubString),
            Contract.StringSearch.Contains => x => x.Contains(stringFilter.SubString),
            Contract.StringSearch.EndsWith => x => x.EndsWith(stringFilter.SubString),
            _ => throw new ArgumentOutOfRangeException()
        };

        var parameter = Expression.Parameter(typeof(TEntity), null);
        var propertyAccess = Expression.Invoke(propertySelector, parameter);
        var containsCall = Expression.Invoke(containsExpression, propertyAccess);
        var lambda = Expression.Lambda<Func<TEntity, bool>>(containsCall, parameter);
        return lambda;
    }


    public static IQueryable<TEntity> WithFilter<TEntity, TProperty>(this IQueryable<TEntity> query, Expression<Func<TEntity, TProperty>> propertySelector, List<TProperty>? inList)
    {
        if (inList is null || inList.Count == 0)
        {
            return query;
        }

        ArgumentNullException.ThrowIfNull(propertySelector, nameof(propertySelector));
        ArgumentNullException.ThrowIfNull(query, nameof(query));

        Expression<Func<TProperty, bool>> containsExpression = x => inList.Contains(x);
        var parameter = Expression.Parameter(typeof(TEntity), null);
        var propertyAccess = Expression.Invoke(propertySelector, parameter);
        var containsCall = Expression.Invoke(containsExpression, propertyAccess);
        var lambda = Expression.Lambda<Func<TEntity, bool>>(containsCall, parameter);
        return query.Where(lambda);
    }


    public static IQueryable<TEntity> WithFilter<TEntity>(this IQueryable<TEntity> query, Expression<Func<TEntity, bool>> propertySelector, bool? value)
    {
        if (!value.HasValue)
        {
            return query;
        }

        ArgumentNullException.ThrowIfNull(propertySelector, nameof(propertySelector));
        ArgumentNullException.ThrowIfNull(query, nameof(query));

        var parameter = Expression.Parameter(typeof(TEntity), null);
        var propertyAccess = Expression.Invoke(propertySelector, parameter);
        var equalCall = Expression.Invoke(Expression.Equal(propertyAccess, Expression.Constant(value.Value)), propertyAccess);
        var lambda = Expression.Lambda<Func<TEntity, bool>>(equalCall, parameter);
        return query.Where(lambda);
    }


    public static IQueryable<TEntity> WhereIf<TEntity>(this IQueryable<TEntity> source, bool condition, Expression<Func<TEntity, bool>> predicate)
    {
        if (!condition)
        {
            return source;
        }

        return source.Where(predicate);
    }

    public static IQueryable<TEntity> WithFilter<TEntity>(this IQueryable<TEntity> query, Expression<Func<TEntity, string>> propertySelector, Contract.StringFilter? stringFilter)
    {
        if (string.IsNullOrEmpty(stringFilter?.SubString))
        {
            return query;
        }

        ArgumentNullException.ThrowIfNull(propertySelector, nameof(propertySelector));
        ArgumentNullException.ThrowIfNull(query, nameof(query));

        var resultExpression = GetStringFilterExpression(propertySelector!, stringFilter);
        return query.Where(resultExpression);
    }


    public static IQueryable<TEntity> WithPaging<TEntity>(this IOrderedQueryable<TEntity> query, Contract.FilterBase filter)
    {
        ArgumentNullException.ThrowIfNull(filter, nameof(filter));

        IQueryable<TEntity> modifiedQuery = query;

        if (filter.Skip > 0)
        {
            modifiedQuery = modifiedQuery.Skip(filter.Skip);
        }

        if (filter.Take > 0) 
        {
            modifiedQuery = modifiedQuery.Take(filter.Take);
        }

        return modifiedQuery;
    }
}