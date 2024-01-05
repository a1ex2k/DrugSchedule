using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Api.Shared.Dttos;
using DrugSchedule.Api.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using DrugSchedule.Api.Utils;
using DrugSchedule.BusinessLogic.Auth;
using DrugSchedule.BusinessLogic.Services;
using Mapster;

namespace DrugSchedule.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly IUserService _userService;

    public AuthController(ITokenService tokenService, IUserService userService)
    {
        _tokenService = tokenService;
        _userService = userService;
    }


    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var loginResult = await _userService.LogUserInAsync(dto.Adapt<LoginModel>());

        if (!loginResult.IsSuccess)
        {
            return Ok(Status.Fail("User was not found", loginResult.Errors));
        }

        var tokens = await _tokenService.CreateTokensAsync(
            loginResult.Result.Identity.Guid, loginResult.Result.Profile.UserProfileId,
            HttpContext.Request.Headers.UserAgent);

        if (tokens is null)
        {
            return Ok(Status.Fail("Token creation failed"));
        }

        return Ok(tokens);
    }


    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var registerResult = await _userService.RegisterUserAsync(dto.Adapt<RegisterModel>());

        if (!registerResult.IsSuccess)
        {
            return Ok(Status.Fail("User was not created", registerResult.Errors));
        }

        return Ok(Status.Success("User created successfully!"));
    }


    [HttpPost]
    public async Task<IActionResult> RefreshToken(TokenDto tokenDto)
    {
        var newTokenModel = await _tokenService.RefreshTokensAsync(tokenDto.Adapt<TokenModel>());

        if (newTokenModel == null)
        {
            return Ok(Status.Fail("Invalid access token or refresh token"));
        }

        return Ok(newTokenModel.Adapt<TokenDto>());
    }


    [HttpPost]
    public async Task<IActionResult> UsernameAvailable(UsernameDto usernameDto)
    {
        var usernameResult = await _userService.IsUsernameAvailableAsync(usernameDto.Username!);

        if (!usernameResult.IsSuccess)
        {
            return Ok(Status.Fail(null, usernameResult.Errors));
        }

        return Ok(usernameResult.Result.Adapt<AvailableUsernameModel>());
    }
}