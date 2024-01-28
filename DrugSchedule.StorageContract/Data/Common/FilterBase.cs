namespace DrugSchedule.StorageContract.Data.Common;

public abstract class FilterBase
{
    public int Skip { get; set; }

    public int Take { get; set; }
}