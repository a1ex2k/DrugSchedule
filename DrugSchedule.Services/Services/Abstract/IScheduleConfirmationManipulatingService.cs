using DrugSchedule.Services.Models;
using OneOf.Types;

namespace DrugSchedule.Services.Services;

public interface IScheduleConfirmationManipulatingService
{
    Task<OneOf<ConfirmationId, NotFound, InvalidInput>> CreateConfirmationAsync(NewTakingСonfirmation newConfirmation, CancellationToken cancellationToken = default);

    Task<OneOf<ConfirmationId, NotFound, InvalidInput>> UpdateConfirmationAsync(TakingСonfirmationUpdate update, CancellationToken cancellationToken = default);

    Task<OneOf<True, NotFound>> RemoveConfirmationAsync(ConfirmationId confirmationId, CancellationToken cancellationToken = default);

    Task<OneOf<DownloadableFile, NotFound, InvalidInput>> AddConfirmationImageAsync(ConfirmationId confirmationId, InputFile inputFile, CancellationToken cancellationToken = default);

    Task<OneOf<True, NotFound>> RemoveConfirmationImageAsync(ConfirmationId confirmationId, Guid fileGuid, CancellationToken cancellationToken = default);
}