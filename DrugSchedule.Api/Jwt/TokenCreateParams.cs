namespace DrugSchedule.Services.Models;

public class TokenCreateParams
{
    public Guid UserGuid { get; set; }

    public long UserProfileId { get; set; }

    public string? ClientInfo { get; set; }
}