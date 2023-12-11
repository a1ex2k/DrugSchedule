using Microsoft.AspNetCore.Mvc;
using DrugSchedule.Api.Models;
using DrugSchedule.Api.Utils;
using DrugSchedule.BusinessLogic.Auth;
using DrugSchedule.BusinessLogic.Services;
using OneOf;


namespace DrugSchedule.Api.Controllers
{
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
        public async Task<OneOf<TokenModel, StatusResponse>> Login([FromBody] LoginModel model)
        {
            var loginResult = await _userService.LogUserInAsync(model);

            if (!loginResult.IsSuccess)
            {
                return Status.Fail("User was not found", loginResult.Errors);
            }

            var tokens = await _tokenService.CreateTokensAsync(
                loginResult.Result.Identity.Guid, loginResult.Result.Profile.UserProfileId,
                HttpContext.Request.Headers.UserAgent);

            return tokens;
        }


        [HttpPost]
        public async Task<StatusResponse> Register([FromBody] RegisterModel model)
        {
            var registerResult = await _userService.RegisterUserAsync(model);

            if (!registerResult.IsSuccess)
            {
                return Status.Fail("User was not created", registerResult.Errors);
            }

            return Status.Success("User created successfully!");
        }


        [HttpPost]
        public async Task<OneOf<TokenModel, StatusResponse>> RefreshToken(TokenModel tokenModel)
        {
            var newTokenModel = await _tokenService.RefreshTokensAsync(tokenModel);

            if (newTokenModel == null)
            {
                return Status.Fail("Invalid access token or refresh token");
            }

            return newTokenModel;
        }
    }
}
