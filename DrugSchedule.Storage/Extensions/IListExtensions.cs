namespace DrugSchedule.Storage.Extensions;

public static class IListExtensions
{
    public static bool IsNullOrEmpty<TItem>(this ICollection<TItem>? list)
    {
        return list is null || list.Count == 0;
    }

    public static void RemoveAndAddExceptExistingByKey<TSourceItem, TNewItem, TKey>(
        this List<TSourceItem> sourceList, 
        List<TNewItem>? newItemList,
        Func<TSourceItem, TKey> sourceKeySelector,
        Func<TNewItem, TKey> newKeySelector,
        Func<TNewItem, TSourceItem> convertor)
    {
        if (newItemList.IsNullOrEmpty())
        {
            sourceList.Clear();
        }

        var commonKeys = sourceList.Select(sourceKeySelector)
            .Except(newItemList!.Select(newKeySelector)).ToList();
        sourceList.RemoveAll(i => commonKeys.Contains(sourceKeySelector(i)));
        var newExceptCommon = newItemList.Where(i => commonKeys.Contains(newKeySelector(i)));
        sourceList.AddRange(newExceptCommon.Select(convertor));
    }
}