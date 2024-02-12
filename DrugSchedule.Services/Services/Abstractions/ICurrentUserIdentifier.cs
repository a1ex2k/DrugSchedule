namespace DrugSchedule.Services.Services.Abstractions;

public interface ICurrentUserIdentifier
{
    bool IsAvailable { get; }

    bool CanBeSet { get; }

    long UserProfileId { get; }

    Guid UserIdentityGuid { get; }

    void Set(Guid userIdentityGuid, long userProfileId);

    void SetNotAvailable();
}