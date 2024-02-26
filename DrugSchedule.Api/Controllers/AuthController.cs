using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Api.Utils;
using Microsoft.AspNetCore.Mvc;
using DrugSchedule.Services.Models;
using DrugSchedule.Services.Services.Abstractions;
using Mapster;
using Microsoft.AspNetCore.Authorization;

namespace DrugSchedule.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly IIdentityService _identityService;

    public AuthController(ITokenService tokenService, IIdentityService identityService)
    {
        _tokenService = tokenService;
        _identityService = identityService;
    }


    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginDto dto, CancellationToken cancellationToken)
    {
        var loginResult = await _identityService.LogUserInAsync(dto.Adapt<LoginModel>(), cancellationToken);
        if (loginResult.IsT1)
        {
            return BadRequest(loginResult.AsT1.ToDto());
        }
 
        var tokenParams = new TokenCreateParams
        {
            UserGuid = loginResult.AsT0.UserIdentityGuid,
            UserProfileId = loginResult.AsT0.UserProfileId,
            ClientInfo = Request.Headers.UserAgent
        };
        var tokensResult = await _tokenService.CreateTokensAsync(tokenParams, cancellationToken);
        if (tokensResult.IsT1)
        {
            return BadRequest(tokensResult.AsT1.ToDto());
        }

        return Ok(tokensResult.AsT0.Adapt<TokenDto>());
    }


    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto, CancellationToken cancellationToken)
    {
        var registerResult = await _identityService.RegisterUserAsync(dto.Adapt<RegisterModel>(), cancellationToken);
        return registerResult.Match<IActionResult>(
            identity => Ok("User created"),
            error => BadRequest(error.ToDto()));
    }
        

    [HttpPost]
    public async Task<IActionResult> RefreshToken(TokenDto tokenDto, CancellationToken cancellationToken)
    {
        var newTokenModel = await _tokenService.RefreshTokensAsync(tokenDto.Adapt<TokenModel>(), cancellationToken);
        return newTokenModel.Match<IActionResult>(
            tokenModel => Ok(tokenModel.Adapt<TokenDto>()),
            error => BadRequest(error.ToDto()));
    }


    [HttpPost]
    [Authorize]
    public async Task<IActionResult> RevokeTokens([FromBody] TokenDto tokenDto, [FromServices] ICurrentUserIdentifier currentUserIdentifier, CancellationToken cancellationToken)
    {
        await _tokenService.RevokeRefreshTokenAsync(currentUserIdentifier.IdentityGuid, tokenDto.RefreshToken, cancellationToken);
        return Ok();
    }


    [HttpPost]
    public async Task<IActionResult> UsernameAvailable(UsernameDto usernameDto, CancellationToken cancellationToken)
    {
        var usernameResult = await _identityService.IsUsernameAvailableAsync(usernameDto.Username!, cancellationToken);
        return Ok(usernameResult.Adapt<AvailableUsername>());
    }
}