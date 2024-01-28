using System.Linq.Expressions;
using DrugSchedule.StorageContract.Data.Common;

namespace DrugSchedule.Storage.Extensions;

public static class IQueryableExtensions
{
    public static IQueryable<TEntity> WithFilter<TEntity, TProperty>(this IQueryable<TEntity> query, Expression<Func<TEntity, TProperty>> propertySelector, List<TProperty>? inList)
    {
        ArgumentNullException.ThrowIfNull(propertySelector, nameof(propertySelector));
        ArgumentNullException.ThrowIfNull(query, nameof(query));

        if (inList is null || inList.Count == 0)
        {
            return query;
        }

        var containsMethod = typeof(List<TProperty>).GetMethod("Contains") ??
                             throw new InvalidOperationException("Contains method not found.");

        var containsCall = Expression.Call(Expression.Constant(inList), containsMethod, propertySelector.Body);
        var predicate = Expression.Lambda<Func<TEntity, bool>>(containsCall, propertySelector.Parameters);

        var compiledPredicate = predicate.Compile();

        return query.Where(compiledPredicate).AsQueryable();
    }

    public static IQueryable<TEntity> WithFilter<TEntity, TProperty>(this IQueryable<TEntity> query, Expression<Func<TEntity, TProperty>> propertySelector, Contract.StringFilter? stringFilter)
    {
        ArgumentNullException.ThrowIfNull(propertySelector, nameof(propertySelector));
        ArgumentNullException.ThrowIfNull(query, nameof(query));

        if (string.IsNullOrEmpty(stringFilter?.SubString))
        {
            return query;
        }

        var containsMethodName = stringFilter.StringSearchType switch
        {
            Contract.StringSearch.StartsWith => "StartsWith",
            Contract.StringSearch.Contains => "Contains",
            Contract.StringSearch.EndsWith => "EndsWith",
            _ => "Contains"
        };

        var containsMethod = typeof(String).GetMethod(containsMethodName) ??
                             throw new InvalidOperationException($"Method {containsMethodName}() not found.");

        var containsCall = Expression.Call(Expression.Constant(stringFilter.SubString), containsMethod, propertySelector.Body);
        var predicate = Expression.Lambda<Func<TEntity, bool>>(containsCall, propertySelector.Parameters);

        var compiledPredicate = predicate.Compile();

        return query.Where(compiledPredicate).AsQueryable();
    }

    public static IQueryable<TEntity> WithPaging<TEntity>(this IQueryable<TEntity> query, FilterBase filter)
    {
        if (filter == null)
        {
            return query;
        }

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