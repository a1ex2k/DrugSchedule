using System.Text.Json.Serialization;

namespace DrugSchedule.Api.Shared.Dtos;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum StringSearchDto : byte
{
    Starts = 0,
    Contains = 1,
    Ends = 2,
}