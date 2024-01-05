using System.Linq.Expressions;
using DrugSchedule.StorageContract.Data.Common;

namespace DrugSchedule.SqlServer.Extensions;

public static class DbSetExtensions
{
    public static EntityRemoveResult thFilter<TEntity, TProperty>(this IQueryable<TEntity> query, Expression<Func<TEntity, TProperty>> propertySelector, IList<TProperty>? inList)
    {
        
    }
}