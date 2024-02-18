using DrugSchedule.Services.Services.Abstractions;

namespace DrugSchedule.Services.Services;

public class CurrentUserIdentifier : ICurrentUserIdentifier
{
    private long _userProfileId;
    private Guid _userIdentityGuid;
    private bool _isAvailable;
    private bool _isSet = false;

    public bool IsAvailable => _isAvailable && _isSet;
    public bool CanBeSet => !_isSet;
    public long UserId => _userProfileId;
    public Guid IdentityGuid => _userIdentityGuid;

    public void Set(Guid identityGuid, long userId)
    {
        if (_isSet)
        {
            throw new InvalidOperationException("Data already set");
        }

        _userIdentityGuid = identityGuid;
        _userProfileId = userId;
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