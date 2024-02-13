namespace DrugSchedule.Services.Options;

public class JwtOptions
{
    public const string SectionName = "JWT";
    public string ValidIssuer { get; set; } = string.Empty;
    public string ValidAudience { get; set; } = string.Empty;
    public string Secret { get; set; } = string.Empty;
    public int AccessTokenValidityInMinutes { get; set; }
    public int RefreshTokenValidityInDays { get; set; }
}