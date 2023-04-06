using Tennis.Model.Models;

namespace Tennis.Model.Results;
public class Result<TSuccess, TFailure>
{
    public TSuccess? Success { get; }
    public TFailure? Failure { get; }
    public bool IsSuccess { get; }
    public int StatusCode { get; }
    public string? Message { get; }

    private Result(TSuccess? success, TFailure? failure, bool isSuccess, int statusCode, string? message = null)
    {
        Success = success;
        Failure = failure;
        IsSuccess = isSuccess;
        StatusCode = statusCode;
        Message = message;
    }

    public static Result<TSuccess, TFailure> FromSuccess(TSuccess success, int statusCode, string? message = null)
    {
        return new Result<TSuccess, TFailure>(success, default, true, statusCode, message);
    }
    public static Result<TSuccess, TFailure> FromFailure(TFailure failure, int statusCode, string? message = null)
    {
        return new Result<TSuccess, TFailure>(default, failure, false, statusCode, message);
    }
}