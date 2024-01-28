namespace DrugSchedule.Storage.Extensions;

public static class IListExtensions
{
    public static bool IsNullOrEmpty<TItem>(this ICollection<TItem>? list)
    {
        return list is null || list.Count == 0;
    }
}