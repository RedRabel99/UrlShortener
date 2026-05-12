namespace UrlShortener.Results;

public record Result
{
    public bool IsSuccess { get; }
    public bool IsError => !IsSuccess;
    public Error Error { get; }

    protected Result(bool isSuccess, Error error)
    {
        if(isSuccess && error != Error.None)
        {
            throw new ArgumentException("Successfull result cant have an error", nameof(error));
        }
        if(!isSuccess && error == Error.None)
        {
            throw new ArgumentException("Fauilure error cant be none", nameof(error));
        }
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, Error.None);
    public static Result Failure(Error error) => new(false, error);
}

public record Result<TValue> : Result
{
    TValue Value { get; }

    public Result(bool isSuccess, TValue value, Error error) : base(isSuccess, error)
    {
            Value = value;
    }

    public static Result<TValue> Success(TValue value) => new(true, value, Error.None);
    public static Result<TValue> Failure(Error error) => new(false, default!, error);
}