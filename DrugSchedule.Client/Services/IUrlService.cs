namespace DrugSchedule.Client.Services;

public interface IUrlService
{
    string? ToApiServerAbsoluteUrl(string? relativeUrl);
}