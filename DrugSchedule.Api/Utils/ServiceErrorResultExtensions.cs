using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.BusinessLogic.Utils;

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
            Messages = new List<string>(1){ notFound.Message },
        };
    }
}