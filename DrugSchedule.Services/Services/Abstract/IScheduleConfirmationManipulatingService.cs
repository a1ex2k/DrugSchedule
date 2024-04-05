using DrugSchedule.Services.Models;
using OneOf.Types;

namespace DrugSchedule.Services.Services;

public interface IScheduleConfirmationManipulatingService
{
    Task<OneOf<ConfirmationIds, NotFound, InvalidInput>> CreateConfirmationAsync(NewTakingСonfirmation newConfirmation, CancellationToken cancellationToken = default);

    Task<OneOf<ConfirmationIds, NotFound, InvalidInput>> UpdateConfirmationAsync(TakingСonfirmationUpdate update, CancellationToken cancellationToken = default);

    Task<OneOf<True, NotFound>> RemoveConfirmationAsync(ConfirmationIds confirmationIds, CancellationToken cancellationToken = default);

    Task<OneOf<DownloadableFile, NotFound, InvalidInput>> AddConfirmationImageAsync(ConfirmationIds confirmationIds, InputFile inputFile, CancellationToken cancellationToken = default);

    Task<OneOf<True, NotFound>> RemoveConfirmationImageAsync(ConfirmationIds confirmationIds, Guid fileGuid, CancellationToken cancellationToken = default);
}