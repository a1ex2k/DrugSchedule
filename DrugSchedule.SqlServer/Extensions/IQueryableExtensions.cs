using System.Linq.Expressions;


namespace DrugSchedule.SqlServer.Extensions;

public static class IQueryableExtensions
{
    public static IQueryable<TEntity> WithFilter<TEntity, TProperty>(this IQueryable<TEntity> query, Expression<Func<TEntity, TProperty>> propertySelector, IList<TProperty>? inList)
    {
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

}