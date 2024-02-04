using System.Linq;
using System.Linq.Expressions;


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

        var parameter = propertySelector.Parameters[0];
        var propertyAccess = Expression.Invoke(propertySelector, parameter);
        var containsCall = Expression.Invoke(containsExpression, propertyAccess);
        var lambda = Expression.Lambda<Func<TEntity, bool>>(containsCall, propertySelector.Parameters[0]);
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
        var parameter = propertySelector.Parameters[0];
        var propertyAccess = Expression.Invoke(propertySelector, parameter);
        var containsCall = Expression.Invoke(containsExpression, propertyAccess);
        var lambda = Expression.Lambda<Func<TEntity, bool>>(containsCall, propertySelector.Parameters[0]);
        return query.Where(lambda);
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


    public static IQueryable<TEntity> WithFilterOrIfNull<TEntity>(this IQueryable<TEntity> query, Expression<Func<TEntity, string?>> propertySelectorFirst, Expression<Func<TEntity, string>> propertySelectorSecond, Contract.StringFilter? stringFilter)
    {
        if (string.IsNullOrEmpty(stringFilter?.SubString))
        {
            return query;
        }

        ArgumentNullException.ThrowIfNull(propertySelectorFirst, nameof(propertySelectorFirst));
        ArgumentNullException.ThrowIfNull(propertySelectorSecond, nameof(propertySelectorSecond));
        ArgumentNullException.ThrowIfNull(query, nameof(query));

        var resultExpression = Expression.Lambda<Func<TEntity, bool>>(
            Expression.Or(
                    Expression.And(
                        Expression.NotEqual(propertySelectorFirst, Expression.Constant(null)), 
                        Expression.Invoke(propertySelectorFirst, GetStringFilterExpression(propertySelectorFirst, stringFilter))
                        ), 
                GetStringFilterExpression(propertySelectorSecond!, stringFilter)));

        return query.Where(resultExpression);
    }


    public static IQueryable<TEntity> WithPaging<TEntity>(this IQueryable<TEntity> query, Contract.FilterBase filter)
    {
        ArgumentNullException.ThrowIfNull(filter, nameof(filter));

        var modifiedQuery = query;

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