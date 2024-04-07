namespace DrugSchedule.Client.Services;

public class UrlService : IUrlService
{
    private readonly HttpClient _httpClient;

    public UrlService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public string? ToApiServerAbsoluteUrl(string? url)
    {
        Uri.TryCreate(_httpClient.BaseAddress, url, out var absoluteUri);
        return absoluteUri?.ToString()!;
    }
}