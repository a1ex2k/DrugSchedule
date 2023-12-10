using Microsoft.AspNetCore.Identity;

namespace DrugSchedule.SqlServer.Data.Entities;

public class RefreshTokenEntry
{
    public long Id { get; set; }

    public IdentityUser? IdentityUser { get; set; }

    public string IdentityUserGuid { get; set; }

    public required string RefreshToken { get; set; }

    public required DateTime RefreshTokenExpiryTime { get; set; }

    public string? ClientInfo { get; set; }
}