using DrugSchedule.SqlServer.Data;
using DrugSchedule.SqlServer.Extensions;
using DrugSchedule.StorageContract.Abstractions;
using DrugSchedule.StorageContract.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Principal;

namespace DrugSchedule.SqlServer.Services;

public class IdentityRepository : IIdentityRepository
{
    private readonly IdentityContext _identityContext;
    private readonly ILogger<IdentityRepository> _logger;
    private readonly UserManager<IdentityUser> _userManager;

    public IdentityRepository(IdentityContext dbContext, UserManager<IdentityUser> userManager, ILogger<IdentityRepository> logger)
    {
        _identityContext = dbContext;
        _userManager = userManager;
        _logger = logger;
    }


    public async Task<UserIdentity?> GetUserIdentityByUsernameAsync(string username)
    {
        var identityUser = await _userManager.FindByNameAsync(username);
        return identityUser?.ToContracUserIdentity();
    }


    public async Task<UserIdentity?> GetUserIdentityByUserIdAsync(Guid userGuid)
    {
        var guidString = userGuid.ToString();
        var identityUser = await _userManager.FindByIdAsync(guidString);
        return identityUser?.ToContracUserIdentity();
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

        return identityUser.ToContracUserIdentity();
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