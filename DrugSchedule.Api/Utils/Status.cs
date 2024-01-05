using DrugSchedule.Api.Shared.Models;

namespace DrugSchedule.Api.Utils;

public static class Status
{
    public static StatusResponseDto Success(string? message)
    {
        return new StatusResponseDto
        {
            Success = true,
            Message = message,
            Errors = null
        };
    }

    public static StatusResponseDto Fail(string? message, List<string>? errors = null)
    {
        return new StatusResponseDto
        {
            Success = false,
            Message = message,
            Errors = errors
        };
    }

}