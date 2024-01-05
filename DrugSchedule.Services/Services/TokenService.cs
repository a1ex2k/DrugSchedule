using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DrugSchedule.BusinessLogic.Auth;
using DrugSchedule.StorageContract.Abstractions;
using Microsoft.Extensions.Options;
using DrugSchedule.BusinessLogic.Options;

namespace DrugSchedule.BusinessLogic.Services;

public class TokenService : ITokenService
{
    private const int RefreshTokenLength = 64;

    private readonly IOptions<JwtOptions> _jwtOptions;
    private readonly IRefreshTokenRepository _tokenRepository;

    public TokenService(IOptions<JwtOptions> jwtOptions, IRefreshTokenRepository tokenRepository)
    {
        _jwtOptions = jwtOptions;
        _tokenRepository = tokenRepository;
    }


    public async Task<TokenModel?> RefreshTokensAsync(TokenModel tokenModel)
    {
        var accessToken = tokenModel.AccessToken;
        var refreshToken = tokenModel.RefreshToken;

        if (string.IsNullOrWhiteSpace(accessToken) || string.IsNullOrWhiteSpace(refreshToken))
        {
            return null;
        }

        var principal = GetPrincipalFromExpiredToken(accessToken);
        if (principal == null)
        {
            return null;
        }

        var userGuidString = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userGuidString) || !Guid.TryParse(userGuidString, out var guid))
        {
            return null;
        }

        var userProfileIdString = principal.Claims.FirstOrDefault(c => c.Type == StringConstants.UserProfileIdClaimName)?.Value;
        if (string.IsNullOrWhiteSpace(userProfileIdString) || !long.TryParse(userProfileIdString, out var userProfileId))
        {
            return null;
        }

        var refreshTokenEntry = await _tokenRepository.GetRefreshTokenEntryAsync(guid, refreshToken!);
        if (refreshTokenEntry is null || refreshTokenEntry.RefreshTokenExpiryTime > DateTime.UtcNow)
        {
            return null;
        }

        var tokens = await CreateTokensAsync(guid, userProfileId, refreshTokenEntry.ClientInfo);

        await _tokenRepository.RemoveRefreshTokenAsync(guid, refreshToken!).ConfigureAwait(false);

        return tokens;
    }

    public async Task<TokenModel?> CreateTokensAsync(Guid userGuid, long userProfileId, string? clientInfo)
    {
        var newAccessToken = CreateAccessTokenString(userGuid, userProfileId);
        var newRefreshToken = await CreateRefreshTokenStringAsync(userGuid, clientInfo);

        return new TokenModel
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }

    public async Task<bool> RevokeRefreshTokenAsync(Guid userGuid, string refreshToken)
    {
        return await _tokenRepository.RemoveRefreshTokenAsync(userGuid, refreshToken);
    }


    private string CreateAccessTokenString(Guid userGuid, long userProfileId)
    {
        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userGuid.ToString()),
            new Claim(StringConstants.UserProfileIdClaimName, userProfileId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Value.Secret));

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Value.ValidIssuer,
            audience: _jwtOptions.Value.ValidAudience,
            expires: DateTime.Now.AddMinutes(_jwtOptions.Value.AccessTokenValidityInMinutes),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private async Task<string> CreateRefreshTokenStringAsync(Guid userGuid, string? clientInfo)
    {
        var newRefreshToken = GenerateBytesString(RefreshTokenLength);

        await _tokenRepository.AddRefreshTokenAsync(new()
        {
            UserGuid = userGuid,
            RefreshToken = newRefreshToken,
            RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtOptions.Value.RefreshTokenValidityInDays),
            ClientInfo = clientInfo
        });

        return newRefreshToken;
    }
    
    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Value.Secret)),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken
            || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
        {
            return null;
        }

        return principal;
    }

    private static string GenerateBytesString(int length)
    {
        var randomNumber = new byte[length];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}