namespace Models.Result;

public class ErrorResult : BaseResult
{
    public ErrorResult(string? message = default) : base(true, message)
    {
    }

    public ErrorResult()
    {
    }
}