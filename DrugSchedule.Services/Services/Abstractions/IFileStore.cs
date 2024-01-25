using DrugSchedule.BusinessLogic.Models;
using DrugSchedule.BusinessLogic.Utils;

namespace DrugSchedule.BusinessLogic.Services;

public interface IFileStore
{
    Task<OneOf<FileData, NotFound>> GetReadStreamAsync(int downloadId, CancellationToken cancellationToken = default);
}