using DrugSchedule.Api.Shared.Dtos;
using DrugSchedule.Services.Errors;

namespace DrugSchedule.Api.Utils;

public static class ServiceErrorResultExtensions
{
    public static InvalidInputDto ToDto(this InvalidInput invalidInput)
    {
        return new InvalidInputDto
        {
            Messages = invalidInput.ErrorsList,
        };
    }

    public static NotFoundDto ToDto(this NotFound notFound)
    {
        return new NotFoundDto
        {
            Messages = notFound.ErrorsList,
        };
    }
}