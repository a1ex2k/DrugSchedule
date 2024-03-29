using Microsoft.Extensions.Primitives;

namespace DrugSchedule.Client.Utils;

public static class QueryParametersDictionaryExtensions
{
    public static bool TryGetQueryParameter<T>(this Dictionary<string, StringValues> dict, string key, out T value)
    {
        value = default!;
        if (dict.TryGetValue(key, out var valueFromQueryString))
        {
            if (typeof(T) == typeof(int) && int.TryParse(valueFromQueryString, out var valueAsInt))
            {
                value = (T)(object)valueAsInt;
                return true;
            }

            if (typeof(T) == typeof(string))
            {
                value = (T)(object)valueFromQueryString.ToString();
                return true;
            }

            if (typeof(T) == typeof(long) && long.TryParse(valueFromQueryString, out var valueAsDecimal))
            {
                value = (T)(object)valueAsDecimal;
                return true;
            }
        }

        return false;
    }
}