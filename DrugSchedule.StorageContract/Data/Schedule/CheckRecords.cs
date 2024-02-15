
namespace DrugSchedule.StorageContract.Data;

public record struct ScheduleAccessCheck(long ScheduleId, long OwnerId, bool IsSharedWith);