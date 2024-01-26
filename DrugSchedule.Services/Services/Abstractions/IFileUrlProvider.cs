namespace DrugSchedule.BusinessLogic.Services.Abstractions;

public interface IFileUrlProvider
{
    Task<Uri> GetPrivateFileUriAsync(FileInfo fileGuid, CancellationToken cancellationToken = default);

    Task<Uri> GetPublicFileUriAsync(FileInfo fileGuid, CancellationToken cancellationToken = default);
}