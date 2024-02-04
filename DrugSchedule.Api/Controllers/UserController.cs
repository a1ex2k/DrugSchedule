using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.BusinessLogic.Models;
using Microsoft.AspNetCore.Mvc;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using DrugSchedule.BusinessLogic.Services.Abstractions;

namespace DrugSchedule.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IUserContactsService _userContactsService;

    public UserController(IUserService userService, IUserContactsService userContactsService)
    {
        _userService = userService;
        _userContactsService = userContactsService;
    }


    [HttpPost]
    public async Task<IActionResult> GetMe(CancellationToken cancellationToken)
    {
        var userModel = await _userService.GetCurrentUserAsync(cancellationToken);
        return Ok(userModel.Adapt<UserFullDto>());
    }

    
    [HttpPost]
    public async Task<IActionResult> SearchForUsers(UserSearchDto dto, CancellationToken cancellationToken)
    {
        var searchResult = await _userService.FindUsersAsync(dto.Adapt<UserSearch>(), cancellationToken);
        return searchResult.Match(
            users => (IActionResult)Ok(users.Adapt<UserPublicCollectionDto>()),
            error => (IActionResult)BadRequest(error));
    }


    [HttpPost]
    public async Task<IActionResult> GetContacts(CancellationToken cancellationToken)
    {
        var contacts = await _userContactsService.GetUserContactsAsync(cancellationToken);
        return Ok(contacts.Adapt<UserContactDto>());
    }


    [HttpPost]
    public async Task<IActionResult> AddContact(NewUserContactDto dto, CancellationToken cancellationToken)
    {
        var contactAddResult = await _userContactsService.AddUserContactAsync(dto.Adapt<NewUserContact>(), cancellationToken);
        return contactAddResult.Match(
            ok => (IActionResult)Ok(),
            errorInput => BadRequest(errorInput),
            notFound => NotFound(notFound));
    }


    [HttpPost]
    public async Task<IActionResult> RemoveContact(UserIdDto dto, CancellationToken cancellationToken)
    {
        var contactRemoveResult = await _userContactsService.RemoveUserContactAsync(dto.Adapt<UserId>(), cancellationToken);
        return contactRemoveResult.Match(
            ok => (IActionResult)Ok(),
            notFound => NotFound(notFound));
    }

    
    [HttpPost]
    public async Task<IActionResult> UpdateProfile(UserUpdateDto dto, CancellationToken cancellationToken)
    {
        var updateResult = await _userService.UpdateProfileAsync(dto.Adapt<UserUpdate>(), cancellationToken);
        return updateResult.Match(
            userModel => (IActionResult)Ok(userModel),
            errorInput => BadRequest(errorInput));
    }


    [HttpPost]
    public async Task<IActionResult> ChangePassword(NewPasswordDto dto, CancellationToken cancellationToken)
    {
        var changePasswordResult = await _userService.UpdatePasswordAsync(dto.Adapt<NewPasswordModel>(), cancellationToken);
        return changePasswordResult.Match(
            ok => (IActionResult)Ok(),
            errorInput => BadRequest(errorInput));
    }


    [HttpPost]
    public async Task<IActionResult> SetAvatar(IFormFile file, CancellationToken cancellationToken)
    {
        var fileModel = new InputFile
        {
            NameWithExtension = file.FileName,
            MediaType = file.ContentType,
            Stream = file.OpenReadStream()
        };

        var setAvatarResult = await _userService.SetAvatarAsync(fileModel, cancellationToken);

        return setAvatarResult.Match(
            fileInfo => (IActionResult)Ok(fileInfo),
            errorInput => BadRequest(errorInput));
    }


    [HttpPost]
    public async Task<IActionResult> RemoveAvatar(FileIdDto dto, CancellationToken cancellationToken)
    {
        var removeAvatarResult = await _userService.RemoveAvatarAsync(dto.Adapt<FileId>(), cancellationToken);
        return removeAvatarResult.Match(
            ok => (IActionResult)Ok(),
            notFound => NotFound(notFound));
    }

}