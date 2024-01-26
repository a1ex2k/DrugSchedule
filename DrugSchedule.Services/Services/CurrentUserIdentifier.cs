using DrugSchedule.BusinessLogic.Services.Abstractions;

namespace DrugSchedule.BusinessLogic.Services;

public class CurrentUserIdentifier : ICurrentUserIdentifier
{
    private long _userProfileId;
    private Guid _userIdentityGuid;
    private bool _isAvailable;
    private bool _isSet = false;

    public bool IsAvailable => _isAvailable && _isSet;
    public bool CanBeSet => !_isSet;
    public long UserProfileId => _userProfileId;
    public Guid UserIdentityGuid => _userIdentityGuid;

    public void Set(Guid userIdentityGuid, long userProfileId)
    {
        if (_isSet)
        {
            throw new InvalidOperationException("Data already set");
        }

        _userIdentityGuid = userIdentityGuid;
        _userProfileId = userProfileId;
        _isAvailable = true;
        _isSet = true;
    }

    public void SetNotAvailable()
    {
        if (_isSet)
        {
            throw new InvalidOperationException("Data already set");
        }

        _isAvailable = false;
        _isSet = true;
    }
}