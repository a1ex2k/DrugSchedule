namespace DrugSchedule.Client.Networking;

public interface ITokenStorage
{
    ValueTask<bool> IsSetAsync();

    string GetAccessToken();

    string GetRefreshToken();

    ValueTask RemoveTokensAsync();

    ValueTask WriteTokensAsync(string accessToken, string refreshToken);
}