using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Services.Errors;

namespace DrugSchedule.Api.Utils;

public static class ServiceErrorResultExtensions
{
    public static ErrorDto ToDto(this InvalidInput invalidInput)
    {
        return new ErrorDto
        {
            Category = "InvalidInput",
            Messages = invalidInput.ErrorsList,
        };
    }

    public static ErrorDto ToDto(this NotFound notFound)
    {
        return new ErrorDto
        {
            Category = "NotFound",
            Messages = notFound.ErrorsList,
        };
    }
}