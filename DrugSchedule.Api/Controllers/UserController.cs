using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Api.Utils;
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

    public UserController(IUserService userService)
    {
        _userService = userService;
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
        return searchResult.Match<IActionResult>(
            users => Ok(users.Adapt<UserPublicCollectionDto>()),
            error => BadRequest(error.ToDto()));
    }


    [HttpPost]
    public async Task<IActionResult> UpdateProfile(UserUpdateDto dto, CancellationToken cancellationToken)
    {
        var updateResult = await _userService.UpdateProfileAsync(dto.Adapt<UserUpdate>(), cancellationToken);
        return updateResult.Match<IActionResult>(
            userModel => Ok(userModel.Adapt<UserUpdateDto>()),
            errorInput => BadRequest(errorInput.ToDto()));
    }


    [HttpPost]
    public async Task<IActionResult> ChangePassword(NewPasswordDto dto, CancellationToken cancellationToken)
    {
        var changePasswordResult = await _userService.UpdatePasswordAsync(dto.Adapt<NewPasswordModel>(), cancellationToken);
        return changePasswordResult.Match<IActionResult>(
            ok => Ok("Password changed successfully"),
            errorInput => BadRequest(errorInput.ToDto()));
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

        return setAvatarResult.Match<IActionResult>(
            file => Ok(file.Adapt<DownloadableFileDto>()),
            errorInput => BadRequest(errorInput.ToDto()));
    }


    [HttpPost]
    public async Task<IActionResult> RemoveAvatar(FileIdDto dto, CancellationToken cancellationToken)
    {
        var removeAvatarResult = await _userService.RemoveAvatarAsync(dto.FileGuid, cancellationToken);
        return removeAvatarResult.Match<IActionResult>(
            ok => Ok("Avatar removed successfully"),
            notFound => NotFound(notFound.ToDto()));
    }

}