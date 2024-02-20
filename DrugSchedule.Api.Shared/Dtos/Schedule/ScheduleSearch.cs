namespace DrugSchedule.Api.Shared.Dtos;

public class ScheduleSearchDto
{
    public required string SubString { get; set; }

    public int Skip { get; set; }

    public int Take { get; set; }
}