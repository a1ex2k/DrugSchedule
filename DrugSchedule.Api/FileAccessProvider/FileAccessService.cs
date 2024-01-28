using System.Security.Cryptography;
using System.Text;
using DrugSchedule.Api.Utils;
using Microsoft.Extensions.Options;

namespace DrugSchedule.Api.FileAccessProvider;

public class FileAccessService : IFileAccessService
{
    private const int AccessKeyLength = 16;
    private readonly PrivateFileAccessOptions _fileAccessOptions;
    private readonly TimeSpan _expiryDelay;

    public FileAccessService(IOptions<PrivateFileAccessOptions> privateFileAccessOptions)
    {
        _fileAccessOptions = privateFileAccessOptions.Value;
        _expiryDelay = TimeSpan.FromSeconds(_fileAccessOptions.ExpirationInSeconds);
    }

    public bool Validate(FileAccessParams accessParams)
    {
        var paramsString = BuildParamsString(accessParams);
        var computedSignature = ComputeSignature(paramsString, _fileAccessOptions.SecretKey);
        var areEqual = string.Equals(computedSignature, accessParams.Signature, StringComparison.Ordinal);
        return areEqual && DateTime.UtcNow.Date.ToUnixTime() <= accessParams.ExpiryTime;
    }

    public FileAccessParams Generate(Guid fileGuid)
    {
        var accessParams = new FileAccessParams
        {
            FileGuid = fileGuid,
            AccessKey = Convert.ToBase64String(RandomNumberGenerator.GetBytes(AccessKeyLength)),
            ExpiryTime = (DateTime.UtcNow + _expiryDelay).ToUnixTime()
        };

        var paramsString = BuildParamsString(accessParams);
        var signature = ComputeSignature(paramsString, _fileAccessOptions.SecretKey);
        accessParams.Signature = signature;
        return accessParams;
    }

    private string BuildParamsString(FileAccessParams accessParams)
    {
        return $"{accessParams.FileGuid}#{accessParams.AccessKey}#{accessParams.ExpiryTime}";
    }

    private string ComputeSignature(string message, string secret)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
        var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
        return Convert.ToBase64String(hashBytes);
    }
}