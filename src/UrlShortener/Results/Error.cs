namespace UrlShortener.Results;

public sealed record Error
{
    public string Code { get; }
    public string Message { get; }
    ErrorType Type { get; }

    private Error(string code, string message, ErrorType type)
    {
        Code = code;
        Message = message;
        Type = type;
    }
    public static readonly Error None =  new(string.Empty, string.Empty, ErrorType.None);
    public static Error NotFound(string code, string message) =>
        new(code, message, ErrorType.NotFound);
    public static Error Forbidden(string code, string message) =>
        new(code, message, ErrorType.Forbidden);
    public static Error Validation(string code, string message) =>
    new(code, message, ErrorType.Validation);
    public static Error Unauthorized(string code, string message) =>
        new(code, message, ErrorType.Unauthorized);
    public static Error Internal(string code, string message) =>
        new(code, message, ErrorType.Internal);
    public static Error Conflict(string code, string message) =>
        new(code, message, ErrorType.Conflict);

}

public enum ErrorType
{
    None,
    Validation,
    Unauthorized,
    Forbidden,
    NotFound,
    Internal,
    Conflict
}