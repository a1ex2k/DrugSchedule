namespace DrugSchedule.BusinessLogic.Services.Abstractions;

public interface IFileUrlProvider
{
    string GetPrivateFileUri(Guid fileGuid, CancellationToken cancellationToken = default);

    string GetPublicFileUri(Guid fileGuid, CancellationToken cancellationToken = default);
}