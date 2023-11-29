using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DrugSchedule.Api.Data.Entities.UserData;

public class UserMedicament
{
    public long Id { get; set; }

    public required string Name { get; set; }

    public int? PackQuantity { get; set; }

    public string? Dosage { get; set; }

    public string? ReleaseForm { get; set; }

    public string? ManufacturerName { get; set; }

    public required int UserProfileId { get; set; }
}