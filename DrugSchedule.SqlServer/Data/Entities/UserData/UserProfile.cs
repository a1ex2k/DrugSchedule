using Microsoft.EntityFrameworkCore;

namespace DrugSchedule.SqlServer.Data.Entities;

[Index(nameof(UserGuid))]
public class UserProfile
{
    public long Id { get; set; }

    public required Guid UserGuid { get; set; }

    public required string? RealName { get; set; }

    public required DateOnly? DateOfBirth { get; set; }

    public long? ImageFileInfoId { get; set; }

    public FileInfo? ImageFileInfo { get; set; }

    public List<UserMedicament> UserMedicaments { get; set; } = new();

    public List<MedicamentTakingSchedule> MedicamentTakingSchedules { get; set; } = new();

    public List<Repeat> TakingRepeats { get; set; } = new();

    public List<UserProfileContact> Contacts { get; set; } = new();
}