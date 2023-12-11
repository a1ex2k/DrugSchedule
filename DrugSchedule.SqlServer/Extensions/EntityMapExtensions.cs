namespace DrugSchedule.SqlServer.Extensions;

public static class EntityMapExtensions
{
    public static Contract.UserIdentity ToContractUserIdentity(this Microsoft.AspNetCore.Identity.IdentityUser identityUser)
    {
        return new Contract.UserIdentity
        {
            Guid = Guid.Parse(identityUser.Id),
            Username = identityUser.UserName,
            Email = identityUser.Email,
            IsEmailConfirmed = identityUser.EmailConfirmed
        };
    }

    public static Contract.FileInfo ToContractFileInfo(this Entities.FileInfo fileInfo)
    {
        return new Contract.FileInfo
        {
            Guid = fileInfo.Guid,
            FileName = fileInfo.FileName,
            ContentType = fileInfo.ContentType,
            Size = fileInfo.Size,
            CreatedAt = fileInfo.CreatedAt,
        };
    }

    public static Contract.UserProfile ToContractUserProfile(this Entities.UserProfile userProfile)
    {
        return new Contract.UserProfile
        {
            UserProfileId = userProfile.Id,
            RealName = userProfile.RealName,
            DateOfBirth = userProfile.DateOfBirth,
            Image = userProfile.ImageFileInfo?.ToContractFileInfo()
        };
    }

}