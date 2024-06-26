﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DrugSchedule.Services;
using DrugSchedule.Services.Errors;
using DrugSchedule.Services.Models;
using DrugSchedule.Services.Options;
using DrugSchedule.Services.Services.Abstractions;
using DrugSchedule.StorageContract.Abstractions;
using DrugSchedule.StorageContract.Data;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OneOf;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace DrugSchedule.Api.Jwt;

public class TokenService : ITokenService
{
    private const int RefreshTokenLength = 64;

    private readonly TokenValidationParameters _tokenValidationParameters;
    private readonly IOptions<JwtOptions> _jwtOptions;
    private readonly IRefreshTokenRepository _tokenRepository;

    public TokenService(IRefreshTokenRepository tokenRepository, TokenValidationParameters tokenValidationParameters, IOptions<JwtOptions> jwtOptions)
    {
        _tokenRepository = tokenRepository;
        _tokenValidationParameters = tokenValidationParameters;
        _jwtOptions = jwtOptions;
    }


    public async Task<OneOf<TokenModel, InvalidInput>> RefreshTokensAsync(TokenModel tokenModel, CancellationToken cancellationToken = default)
    {
        var accessToken = tokenModel.AccessToken;
        var refreshToken = tokenModel.RefreshToken;
        var invalidTokenError = new InvalidInput("Invalid token(s)");

        if (string.IsNullOrWhiteSpace(accessToken) || string.IsNullOrWhiteSpace(refreshToken))
        {
            return invalidTokenError;
        }

        var principal = GetPrincipalFromExpiredToken(accessToken);
        if (principal == null)
        {
            return invalidTokenError;
        }

        var userGuidString = principal.Claims.FirstOrDefault(c => c.Type == StringConstants.UserIdentityGuidClaimName)?.Value;
        if (userGuidString == null || !Guid.TryParse(userGuidString, out var guid))
        {
            return invalidTokenError;
        }

        var userProfileIdString = principal.Claims.FirstOrDefault(c => c.Type == StringConstants.UserProfileIdClaimName)?.Value;
        if (string.IsNullOrWhiteSpace(userProfileIdString) || !long.TryParse(userProfileIdString, out var userProfileId))
        {
            return invalidTokenError;
        }

        var refreshTokenEntry = await _tokenRepository.GetRefreshTokenEntryAsync(guid, refreshToken!, cancellationToken);
        if (refreshTokenEntry is null 
            || refreshTokenEntry.RefreshTokenExpiryTime < DateTime.UtcNow)
        {
            return invalidTokenError;
        }

        var tokenParams = new TokenCreateParams
        {
            UserGuid = guid,
            UserProfileId = userProfileId,
            ClientInfo = refreshTokenEntry.ClientInfo
        };

        var newTokenModel = await CreateTokensInternalAsync(tokenParams, cancellationToken);
        await _tokenRepository.RemoveRefreshTokenAsync(guid, refreshToken!, cancellationToken).ConfigureAwait(false);

        return newTokenModel;
    }

    public async Task<OneOf<TokenModel, InvalidInput>> CreateTokensAsync(TokenCreateParams parameters, CancellationToken cancellationToken = default)
    {
        var newTokenModel = await CreateTokensInternalAsync(parameters, cancellationToken);
        return newTokenModel!;
    }

    private async Task<TokenModel?> CreateTokensInternalAsync(TokenCreateParams parameters, CancellationToken cancellationToken = default)
    {
        var newAccessToken = CreateAccessTokenString(parameters.UserGuid, parameters.UserProfileId);
        var newRefreshToken = await CreateRefreshTokenStringAsync(parameters.UserGuid, parameters.ClientInfo, cancellationToken);

        if (newRefreshToken == null)
        {
            return null;
        }
        return new TokenModel
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }

    public async Task<bool> RevokeRefreshTokenAsync(Guid userGuid, string refreshToken, CancellationToken cancellationToken = default)
    {
        var removeResult = await _tokenRepository.RemoveRefreshTokenAsync(userGuid, refreshToken, cancellationToken);
        return removeResult == RemoveOperationResult.Removed;
    }


    private string CreateAccessTokenString(Guid userGuid, long userProfileId)
    {
        var authClaims = new List<Claim>
        {
            new (StringConstants.UserIdentityGuidClaimName, userGuid.ToString()),
            new (StringConstants.UserProfileIdClaimName, userProfileId.ToString()),
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Value.Secret));

        var token = new JwtSecurityToken(
            issuer: _tokenValidationParameters.ValidIssuer,
            audience: _tokenValidationParameters.ValidAudience,
            expires: DateTime.Now.AddMinutes(_jwtOptions.Value.AccessTokenValidityInMinutes),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private async Task<string?> CreateRefreshTokenStringAsync(Guid userGuid, string? clientInfo, CancellationToken cancellationToken = default)
    {
        var newRefreshToken = GenerateBytesString(RefreshTokenLength);
        var saved = await _tokenRepository.AddRefreshTokenAsync(new()
        {
            UserGuid = userGuid,
            RefreshToken = newRefreshToken,
            RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtOptions.Value.RefreshTokenValidityInDays),
            ClientInfo = clientInfo
        }, cancellationToken);
        return saved?.RefreshToken;
    }

    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token, CancellationToken cancellationToken = default)
    {
        var tokenValidationParameters = _tokenValidationParameters.Clone();
        tokenValidationParameters.ValidateLifetime = false;
        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var principal =
                tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken
                || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            return principal;
        }
        catch { }
        
        return null;
    }

    private static string GenerateBytesString(int length)
    {
        var randomNumber = new byte[length];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}