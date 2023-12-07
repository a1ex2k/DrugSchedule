using Microsoft.EntityFrameworkCore;

namespace DrugSchedule.SqlServer.Extensions;

public static class IQueryableExtensions
{
    public static IQueryable<T> WithFilter<T> (this IQueryable<T> query, IList<TFilter>)

}