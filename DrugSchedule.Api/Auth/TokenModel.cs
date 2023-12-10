namespace DrugSchedule.Api.Auth
{
    public class TokenModel
    {
        public string? AccessToken { get; set; }

        public string? RefreshToken { get; set; }
    }
}
