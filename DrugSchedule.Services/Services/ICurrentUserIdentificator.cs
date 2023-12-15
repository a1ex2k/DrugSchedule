namespace DrugSchedule.BusinessLogic.Services;

public interface ICurrentUserIdentificator
{
    bool IsAvailable { get; }

    bool CanBeSet { get; }

    long UserProfileId { get; }

    Guid UserIdentityGuid { get; }

    void Set(Guid userIdentityGuid, long userProfileId);

    void SetNotAvailable();
}