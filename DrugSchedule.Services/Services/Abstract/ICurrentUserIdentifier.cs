namespace DrugSchedule.Services.Services.Abstractions;

public interface ICurrentUserIdentifier
{
    bool IsAvailable { get; }

    bool CanBeSet { get; }

    long UserId { get; }

    Guid IdentityGuid { get; }

    void Set(Guid identityGuid, long userId);

    void SetNotAvailable();
}