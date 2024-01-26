namespace DrugSchedule.Api.Services;

public interface IFileAssociateService
{
    Task<FileAssociation> AssociateWithIdentityAsync(Guid fileGuid, Guid identityGuid, CancellationToken cancellationToken = default);

    Task<FileAssociation> AssociateAsync(Guid fileGuid, CancellationToken cancellationToken = default);

    Task<FileAssociation> GetFileAssociationAsync(string accessKey, CancellationToken cancellationToken = default);
}

public class FileAssociation
{
    public Guid FileGuid { get; set; }

    public Guid RequestedIdentityGuid { get; set; }
}