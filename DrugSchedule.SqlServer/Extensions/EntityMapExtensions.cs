using System.Linq.Expressions;
using DrugSchedule.StorageContract.Data;
using Microsoft.AspNetCore.Identity;

namespace DrugSchedule.SqlServer.Extensions;

public static class EntityMapExtensions
{
    public static UserIdentity ToContracUserIdentity(this IdentityUser identityUser)
    {
        return new UserIdentity
        {
            Guid = Guid.Parse(identityUser.Id),
            Username = identityUser.UserName,
            Email = identityUser.Email,
            IsEmailConfirmed = identityUser.EmailConfirmed
        };
    }

}