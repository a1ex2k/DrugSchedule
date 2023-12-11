using DrugSchedule.Api.Models;

namespace DrugSchedule.Api.Utils;

public static class Status
{
    public static StatusResponse Success(string? message)
    {
        return new StatusResponse
        {
            Success = true,
            Message = message,
            Errors = null
        };
    }

    public static StatusResponse Fail(string? message, List<string>? errors = null)
    {
        return new StatusResponse
        {
            Success = false,
            Message = message,
            Errors = errors
        };
    }

}