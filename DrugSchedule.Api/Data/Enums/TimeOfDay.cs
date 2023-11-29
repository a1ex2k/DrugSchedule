namespace DrugSchedule.Api.Data;

public enum TimeOfDay
{
    None = 0,
    MorningWakeup,

    BeforeBreakfast,
    DuringBreakFast,
    AfterBreakfast,

    BeforeBrunch,
    DuringBrunch,
    AfterBrunch,

    BeforeLunch,
    DuringLunch,
    AfterLunch,

    BeforeDunch,
    DuringDunch,
    AfterDunch,

    BeforeSleep
}