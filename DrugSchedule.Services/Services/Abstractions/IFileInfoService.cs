using DrugSchedule.BusinessLogic.Models;
using DrugSchedule.BusinessLogic.Utils;



namespace DrugSchedule.BusinessLogic.Services;

public interface IFileInfoService
{
    Task<OneOf<FileInfo, NotFound>> GetFileInfoAsync(Guid fileGuid, CancellationToken cancellationToken = default);

    Task<List<FileInfo>> GetFileInfosAsync(List<Guid> fileGuids, CancellationToken cancellationToken = default);

    Task<OneOf<FileInfo, InvalidInput>> CreateAsync(NewFile newFileInfoModel, CancellationToken cancellationToken = default);
}