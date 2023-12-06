namespace DrugSchedule.SqlServer.Data.Entities;

public class UserProfileContact
{
    public long Id { get; set; }

    public required int UserProfileId { get; set; }

    public UserProfile? UserProfile { get; set; }

    public required int ContactProfileId { get; set; }

    public UserProfile? ContactProfile { get; set; }

    public required string Name { get; set; }

    public List<MedicamentTakingSchedule> SharedSchedules { get; set; } = new();
}