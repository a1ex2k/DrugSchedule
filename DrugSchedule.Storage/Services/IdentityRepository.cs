using DrugSchedule.Storage.Extensions;
using DrugSchedule.Storage.Data;
using DrugSchedule.StorageContract.Abstractions;
using DrugSchedule.StorageContract.Data;
using DrugSchedule.StorageContract.Data.UserStorage;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace DrugSchedule.Storage.Services;

public class IdentityRepository : IIdentityRepository
{
    private readonly DrugScheduleContext _dbContext;
    private readonly UserManager<IdentityUser> _userManager;

    public IdentityRepository(DrugScheduleContext dbContext, UserManager<IdentityUser> userManager, CancellationToken cancellationToken = default)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }


    public async Task<UserIdentity?> GetUserIdentityAsync(string username, CancellationToken cancellationToken = default)
    {
        var identityUser = await _userManager.FindByNameAsync(username);
        return identityUser?.ToContractModel();
    }

    public async Task<UserIdentity?> GetUserIdentityAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        var identityUser = await _userManager.FindByNameAsync(username);

        if (identityUser == null)
        {
            return null;
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(identityUser, password);
        return isPasswordValid ? identityUser?.ToContractModel() : null;
    }


    public async Task<UserIdentity?> GetUserIdentityAsync(Guid userGuid, CancellationToken cancellationToken = default)
    {
        var guidString = userGuid.ToString();
        var identityUser = await _userManager.FindByIdAsync(guidString);
        return identityUser?.ToContractModel();
    }

    public async Task<List<UserIdentity>> GetUserIdentitiesAsync(UserIdentityFilter filter, CancellationToken cancellationToken = default)
    {
        var guids = filter.GuidsFilter?.ConvertAll(g => g.ToString());

        var identityUsers = await _dbContext.Users
            .AsNoTracking()
            .WithFilter(i => i.Id, guids)
            .WithFilter(i => i.UserName!, filter.UsernameFilter)
            .WithPaging(filter)
            .Select(EntityMapExpressions.ToIdentity)
            .OrderBy(identity3 => identity3.Username)
            .ToListAsync(cancellationToken);
        return identityUsers;
    }

    public async Task<List<UserIdentity>> GetUserIdentitiesAsync(string usernameSearchString, CancellationToken cancellationToken = default)
    {
        var identityUsers = await _dbContext.Users
            .AsNoTracking()
            .Where(i => i.UserName != null && i.UserName.Contains(usernameSearchString))
            .Select(EntityMapExpressions.ToIdentity)
            .ToListAsync(cancellationToken);
        return identityUsers;
    }


    public async Task<bool> IsUsernameUsedAsync(string username, CancellationToken cancellationToken = default)
    {
        var normalizedName = _userManager.NormalizeName(username);
        return await _userManager.Users.AnyAsync(u => u.NormalizedUserName == normalizedName, cancellationToken);
    }


    public async Task<bool> IsEmailUsedAsync(string email, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = _userManager.NormalizeEmail(email);
        return await _userManager.Users.AnyAsync(u => u.NormalizedEmail == normalizedEmail, cancellationToken);
    }


    public async Task<UserIdentity> CreateUserIdentityAsync(NewUserIdentity newIdentity, CancellationToken cancellationToken = default)
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

        return identityUser.ToContractModel();
    }


    public async Task<bool> UpdatePasswordAsync(PasswordUpdate passwordUpdate, CancellationToken cancellationToken = default)
    {
        var identityUser = await _userManager.FindByIdAsync(passwordUpdate.IdentityGuid.ToString());
        if (identityUser == null)
        {
            return false;
        }

        var result = await _userManager.ChangePasswordAsync(identityUser, passwordUpdate.OldPassword, passwordUpdate.NewPassword);
        return result.Succeeded;
    }
}