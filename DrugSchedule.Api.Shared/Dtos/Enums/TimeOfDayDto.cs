using System.Text.Json.Serialization;

namespace DrugSchedule.Api.Shared.Dtos;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TimeOfDayDto : byte
{
    None = 0,
    MorningWakeup = 1,

    BeforeBreakfast = 11,
    DuringBreakFast = 12,
    AfterBreakfast = 13,

    BeforeBrunch = 31,
    DuringBrunch = 70,
    AfterBrunch = 80,

    BeforeLunch = 90,
    DuringLunch = 100,
    AfterLunch = 110,

    BeforeNap = 120,
    AfterNap = 121,

    BeforeDunch = 140,
    DuringDunch = 150,
    AfterDunch = 160,

    BeforeDinner = 170,
    DuringDinner = 180,
    AfterDinner = 190,

    BeforeSleep = 255
}