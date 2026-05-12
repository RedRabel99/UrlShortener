namespace UrlShortener.Results;

public record Result
{
    public bool IsSuccess { get; }
    public bool IsError => !IsSuccess;
    public Error Error { get; }

    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
        {
            throw new ArgumentException("Successful result cannot have an error", nameof(error));
        }
        if (!isSuccess && error == Error.None)
        {
            throw new ArgumentException("Failure result must have an error", nameof(error));
        }
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, Error.None);
    public static Result Failure(Error error) => new(false, error);
}

public record Result<TValue> : Result
{
    public TValue Value { get; }

    private Result(bool isSuccess, TValue value, Error error) : base(isSuccess, error)
    {
        Value = value;
    }

    public static Result<TValue> Success(TValue value) => new(true, value, Error.None);
    public static new Result<TValue> Failure(Error error) => new(false, default!, error);
}
