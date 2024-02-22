using DrugSchedule.Api.Shared.Dtos;
namespace DrugSchedule.Client.Networking;

public class ApiCallResult<T>
{
    public bool IsOk { get; init; }

    public T Result { get; init; } = default!;

    public InvalidInputDto? InvalidInput { get; init; }

    public NotFoundDto? NotFound { get; init; }

    public ApiCallResult(T result)
    {
        Result = result;
    }

    public ApiCallResult(InvalidInputDto invalidInput)
    {
        InvalidInput = invalidInput;
    }

    public ApiCallResult(NotFoundDto notFound)
    {
        NotFound = notFound;
    }
}


public class ApiCallResult<T>
{
    public bool IsOk { get; init; }

    public T Result { get; init; } = default!;

    public InvalidInputDto? InvalidInput { get; init; }

    public NotFoundDto? NotFound { get; init; }

    public ApiCallResult(T result)
    {
        Result = result;
    }

    public ApiCallResult(InvalidInputDto invalidInput)
    {
        InvalidInput = invalidInput;
    }

    public ApiCallResult(NotFoundDto notFound)
    {
        NotFound = notFound;
    }
}