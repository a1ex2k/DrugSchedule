using System.Linq.Expressions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace DrugSchedule.Storage.Extensions;

public static class DbContextExtensions
{
    public static void UpdateIf<TEntity, TProperty>(this EntityEntry<TEntity> entry, 
        Expression<Func<TEntity, TProperty>> propertySelector, 
        TProperty value, bool condition) 
        where TEntity : class
    {
        if (!condition)
        {
            return;
        }

        entry.Property(propertySelector).CurrentValue = value;
    }

    public static async Task<bool> TrySaveChangesAsync(this DbContext context, ILogger logger, CancellationToken cancellationToken) 
    {
        ArgumentNullException.ThrowIfNull(context);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (ex.GetBaseException() is SqlException sqlException && sqlException.Number == 547)
        {
            logger.LogError(ex, null);
            return false;
        }

        return true;
    }

}