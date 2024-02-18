using DrugSchedule.Services.Models;
using OneOf.Types;

namespace DrugSchedule.Services.Services;

public interface IScheduleConfirmationManipulatingService
{
    Task<OneOf<ConfirmationId, NotFound, InvalidInput>> CreateConfirmationAsync(NewTakingСonfirmation newConfirmation, CancellationToken cancellationToken = default);

    Task<OneOf<ConfirmationId, NotFound, InvalidInput>> UpdateConfirmationAsync(TakingСonfirmationUpdate update, CancellationToken cancellationToken = default);

    Task<OneOf<True, NotFound>> RemoveConfirmationAsync(ConfirmationId confirmationId, CancellationToken cancellationToken = default);

    Task<OneOf<DownloadableFile, NotFound, InvalidInput>> AddConfirmationImageAsync(long medicamentId, InputFile inputFile, CancellationToken cancellationToken = default);

    Task<OneOf<True, NotFound>> RemoveConfirmationImageAsync(long medicamentId, Guid fileGuid, CancellationToken cancellationToken = default);
}