namespace Models.Result;

public class BaseResult : BaseResult<object>
{
    protected BaseResult(bool error, string? message = default) : base(error, message)
    {
    }

    protected BaseResult()
    {
    }
}

public class BaseResult<TResult>
{
    protected BaseResult(bool error, string? message = default, TResult? result = default)
    {
        Error = error;
        Message = message;
        Result = result;
    }

    protected BaseResult()
    {
    }

    public bool Error { get; set; }
    public string? Message { get; set; }
    public TResult? Result { get; set; }
}