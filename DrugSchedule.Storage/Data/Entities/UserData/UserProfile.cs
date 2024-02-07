namespace DrugSchedule.Storage.Data.Entities;

public class UserProfile
{
    public long Id { get; set; }

    public required string IdentityGuid { get; set; }

    public string? RealName { get; set; }

    public required DateOnly? DateOfBirth { get; set; }

    public required Contract.Sex Sex { get; set; }

    public required Guid? AvatarGuid { get; set; }

    public FileInfo? AvatarInfo { get; set; }

    public List<UserMedicament> UserMedicaments { get; set; } = new();

    public List<MedicamentTakingSchedule> MedicamentTakingSchedules { get; set; } = new();

    public List<ScheduleRepeat> ScheduleRepeats { get; set; } = new();

    public List<UserProfileContact> Contacts { get; set; } = new();
}