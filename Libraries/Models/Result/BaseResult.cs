namespace Models.Result;

public class BaseResult : BaseResult<object>
{
    public BaseResult(bool error, string? message = default) : base(error, message)
    {
    }

    public BaseResult()
    {
    }
}

public class BaseResult<TResult>
{
    public BaseResult(bool error, string? message = default, TResult? result = default)
    {
        Error = error;
        Message = message;
        Result = result;
    }

    public BaseResult()
    {
    }

    public bool Error { get; set; }
    public string? Message { get; set; }
    public TResult? Result { get; set; }
}