
namespace DrugSchedule.StorageContract.Data;

public class ScheduleAccessCheck
{
    public long ScheduleId { get; set; }

    public long OwnerId { get; set; }
    
    public bool IsSharedWith { get; set; }
}