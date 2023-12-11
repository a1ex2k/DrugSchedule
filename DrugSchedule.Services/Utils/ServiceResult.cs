namespace DrugSchedule.BusinessLogic.Utils;

public class ServiceResult<T>
{
    public bool IsSuccess { get; init; }
 
    public T? Result { get; init; }

    public List<string>? Errors { get; init; }

    public ServiceResult(bool isSuccess, T? result, List<string>? errors)
    {
        IsSuccess = isSuccess;
        Result = result;
        Errors = errors;
    }

    public static ServiceResult<T> Success(T result)
    {
        return new ServiceResult<T>(true, result, null);
    }

    public static ServiceResult<T> Fail(List<string> errors)
    {
        return new ServiceResult<T>(false, default(T), errors);
    }

    public static ServiceResult<T> Fail(string error)
    {
        return new ServiceResult<T>(false, default(T), new(){error});
    }
}