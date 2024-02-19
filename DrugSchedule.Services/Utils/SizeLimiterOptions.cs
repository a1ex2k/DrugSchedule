namespace DrugSchedule.Services.Utils;

public class SizeLimiterOptions
{
    public const string SectionName = "Limits";

    public int SimpleCollectionMaxSize { get; set; }

    public int ExtendedCollectionMaxSize { get; set; }

    public int StringLengthLimit { get; set; }
}