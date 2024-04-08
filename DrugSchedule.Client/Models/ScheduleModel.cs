using DrugSchedule.Api.Shared.Dtos;

namespace DrugSchedule.Client.Models;

public class ScheduleModel
{
    public long ScheduleId { get; set; }

    public bool IsNew => ScheduleId == default;

    public MedicamentSimpleDto? GlobalMedicament { get; set; }

    public UserMedicamentSimpleDto? UserMedicament { get; set; }

    public string? Information { get; set; }

    public bool Enabled { get; set; } = true;

    public List<ScheduleShareModel> NewShares { get; set; } = new();

    public List<ScheduleShareExtendedDto> DeleteShares { get; set; } = new();

    public List<KeyValuePair<long, ScheduleRepeatDto>> Repeats { get; set; } = new();
}