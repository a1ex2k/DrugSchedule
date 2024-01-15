using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DrugSchedule.Storage.Extensions;

public static class DbContextExtensions
{
    //public static async Task<bool> TryRemoveAndSaveAsync<TEntity>(this DbContext context, TEntity entity, ILogger logger, CancellationToken cancellationToken) where TEntity : class
    //{
    //    ArgumentNullException.ThrowIfNull(context);
    //    ArgumentNullException.ThrowIfNull(entity);

    //    context.Remove(entity);

    //    try
    //    {
    //        await context.SaveChangesAsync(cancellationToken);
    //    }
    //    catch (DbUpdateException ex) when (ex.GetBaseException() is SqlException sqlException && sqlException.Number == 547)
    //    {
    //        logger.LogError(ex, null);
    //        return false;
    //    }

    //    return true;
    //}

    //public static async Task<bool> TryCreateAndSaveAsync<TEntity>(this DbContext context, TEntity entity, ILogger logger, CancellationToken cancellationToken) where TEntity : class
    //{
    //    ArgumentNullException.ThrowIfNull(context);
    //    ArgumentNullException.ThrowIfNull(entity);

    //    await context.AddAsync(entity, cancellationToken);

    //    try
    //    {
    //        await context.SaveChangesAsync(cancellationToken);
    //    }
    //    catch (DbUpdateException ex) when (ex.GetBaseException() is SqlException sqlException && sqlException.Number == 547)
    //    {
    //        logger.LogError(ex, null);
    //        return false;
    //    }

    //    return true;
    //}

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