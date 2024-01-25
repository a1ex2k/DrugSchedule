namespace DrugSchedule.BusinessLogic.Services.Abstractions;

public interface IFileAssociatingService
{
    Task<FileAssociation> AssociateToIdentityAsync(Guid identityGuid, Guid fileGuid, CancellationToken cancellationToken = default);

    Task<FileAssociation> AssociateAsync(Guid fileGuid, CancellationToken cancellationToken = default);

    Task<FileAssociation> GetFileAssociatedAsync(long associatedId, CancellationToken cancellationToken = default);
}

public class FileAssociation
{
    public required string AssociationId { get; set; }

    public Guid FileGuid { get; set; }

    public Guid UserProfileId { get; set; }

    public DateTime Creation { get; set; }
}