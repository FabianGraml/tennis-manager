namespace Tennis.Model.Models;
public class ResultModel<T>
{
    public bool IsSuccess { get; set; }
    public T? Value { get; set; }
    public string? Error { get; set; }
    public int StatusCode { get; set; }

    public static ResultModel<T> Success(T value, int statusCode = 200)
    {
        return new ResultModel<T> { IsSuccess = true, Value = value, StatusCode = statusCode };
    }

    public static ResultModel<T> Failure(string error, int statusCode = 400)
    {
        return new ResultModel<T> { IsSuccess = false, Error = error, StatusCode = statusCode };
    }
}
