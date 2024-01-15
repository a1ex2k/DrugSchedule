using System;

namespace DrugSchedule.StorageContract.Data;

public class RefreshTokenEntry
{
    public required Guid UserGuid { get; set; }

    public required string RefreshToken { get; set; }

    public DateTime RefreshTokenExpiryTime { get; set; }

    public string? ClientInfo { get; set; }
}