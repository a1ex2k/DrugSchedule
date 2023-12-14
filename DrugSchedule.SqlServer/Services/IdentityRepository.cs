using DrugSchedule.SqlServer.Data;
using DrugSchedule.SqlServer.Extensions;
using DrugSchedule.StorageContract.Abstractions;
using DrugSchedule.StorageContract.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DrugSchedule.SqlServer.Services;

public class IdentityRepository : IIdentityRepository
{
    private readonly DrugScheduleContext _dbContext;
    private readonly UserManager<IdentityUser> _userManager;

    public IdentityRepository(DrugScheduleContext dbContext, UserManager<IdentityUser> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }


    public async Task<UserIdentity?> GetUserIdentityAsync(string username)
    {
        var identityUser = await _userManager.FindByNameAsync(username);
        return identityUser?.ToContractUserIdentity();
    }

    public async Task<UserIdentity?> GetUserIdentityAsync(string username, string password)
    {
        var identityUser = await _userManager.FindByNameAsync(username);

        if (identityUser == null)
        {
            return null;
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(identityUser, password);
        return isPasswordValid ? identityUser?.ToContractUserIdentity() : null;
    }


    public async Task<UserIdentity?> GetUserIdentityAsync(Guid userGuid)
    {
        var guidString = userGuid.ToString();
        var identityUser = await _userManager.FindByIdAsync(guidString);
        return identityUser?.ToContractUserIdentity();
    }


    public async Task<bool> IsUsernameUsedAsync(string username)
    {
        var normalizedName = _userManager.NormalizeName(username);
        return await _userManager.Users.AnyAsync(u => u.NormalizedUserName == normalizedName);
    }


    public async Task<bool> IsEmailUsedAsync(string email)
    {
        var normalizedEmail = _userManager.NormalizeEmail(email);
        return await _userManager.Users.AnyAsync(u => u.NormalizedEmail == normalizedEmail);
    }


    public async Task<UserIdentity> CreateUserIdentityAsync(NewUserIdentity newIdentity)
    {
        var identityUser = new IdentityUser
        {
            UserName = newIdentity.Username,
            Email = newIdentity.Email
        };

        var resultIdentity = await _userManager.CreateAsync(identityUser, newIdentity.Password);

        if (!resultIdentity.Succeeded)
        {
            throw new AggregateException("Cannot save user", resultIdentity.Errors.Select(e => new Exception(e.Description)));
        }

        return identityUser.ToContractUserIdentity();
    }


    public async Task<bool> UpdatePasswordAsync(Guid userGuid, string oldPassword, string newPassword)
    {
        var identityUser = await _userManager.FindByIdAsync(userGuid.ToString());
        if (identityUser == null)
        {
            return false;
        }

        var result = await _userManager.ChangePasswordAsync(identityUser, oldPassword, newPassword);
        return result.Succeeded;
    }
}