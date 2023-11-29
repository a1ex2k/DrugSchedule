using DrugSchedule.Api.Data.Entities.UserData;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DrugSchedule.Api.Data;

public class MedicamentTakingSchedule
{
    public long Id { get; set; }

    public required int UserProfileId { get; set; }

    public UserProfile UserProfile { get; set; }

    public int? MedicamentId { get; set; }

    public Medicament? Medicament { get; set; }

    public int? UserMedicamentId { get; set; }

    public UserMedicament? UserMedicament { get; set; }

    public required string Information { get; set; }

    public required DateTime CreationTime { get; set; }

    public List<Repeat> TakingRepeats { get; set; } = new();
}