namespace DrugSchedule.SqlServer.Data.Entities;

public class UserProfile
{
    public int Id { get; set; }

    public required int UserId { get; set; }

    public User? User { get; set; }

    public required string? RealName { get; set; }

    public required DateOnly? DateOfBirth { get; set; }

    public List<UserMedicament> UserMedicaments { get; set; } = new();

    public List<MedicamentTakingSchedule> MedicamentTakingSchedules { get; set; } = new();

    public List<Repeat> TakingRepeats { get; set; } = new();

    public List<UserProfileContact> Contacts { get; set; } = new();

    public Guid? ImageGuid { get; set; }
}