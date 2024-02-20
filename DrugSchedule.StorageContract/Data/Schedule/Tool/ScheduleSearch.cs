namespace DrugSchedule.StorageContract.Data;

public class ScheduleSearch : FilterBase
{
    public required string SubString { get; set; }
}