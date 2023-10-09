namespace Models.Result;

public class SuccessResult : BaseResult
{
    public SuccessResult(string? message = default) : base(false, message)
    {
    }
}

public class SuccessResult<TResult> : BaseResult<TResult>
{
    public SuccessResult(string? message = default, TResult? result = default) : base(false, message, result)
    {
    }
}