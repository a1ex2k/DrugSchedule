using DrugSchedule.Api.Shared.Dtos;
namespace DrugSchedule.Client.Networking;

public class ApiCallResult
{
    public bool IsOk { get; init; }

    public IEnumerable<string> Messages => InvalidInput?.Messages ?? NotFound?.Messages ?? [];

    public InvalidInputDto? InvalidInput { get; init; }

    public NotFoundDto? NotFound { get; init; }

    public ApiCallResult()
    {
        IsOk = true;
    }

    public ApiCallResult(bool isOk)
    {
        IsOk = isOk;
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


public class ApiCallResult<T> : ApiCallResult
{
    public T ResponseDto { get; init; } = default!;

    public ApiCallResult() : base(false)
    {
    }

    public ApiCallResult(T responseDto) : base()
    {
        ResponseDto = responseDto;
    }

    public ApiCallResult(InvalidInputDto invalidInput) : base(invalidInput)
    {
    }

    public ApiCallResult(NotFoundDto notFound) : base(notFound)
    {
    }
}