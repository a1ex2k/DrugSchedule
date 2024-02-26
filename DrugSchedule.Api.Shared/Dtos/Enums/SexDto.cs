using System.Text.Json.Serialization;

namespace DrugSchedule.Api.Shared.Dtos;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SexDto : byte
{
    Undefined = 0,
    Male = 1,
    Female = 2
}