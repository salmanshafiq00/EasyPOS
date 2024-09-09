using System.Text.Json.Serialization;

namespace EasyPOS.Domain.Shared;

public readonly struct Error
{
    public string Code { get; }
    public string Description { get; }
    [JsonIgnore]
    public ErrorType ErrorType { get; }

    public Error(string code, string description, ErrorType errorType)
    {
        Code = code;
        Description = description;
        ErrorType = errorType;
    }
    public Error()
    {

    }

    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Failure);
    public static readonly Error NullValue = new("Error.NullValue", "Null value was provided", ErrorType.Failure);

    public static implicit operator string(Error error) => error.Code ?? string.Empty;

    //public static implicit operator Result(Error error) => Result.Failure(error);

    public static Error Failure(string code, string description)
        => new(code, description, ErrorType.Failure);

    public static Error Validation(string code, string description)
        => new(code, description, ErrorType.Validation);

    public static Error Unauthorized(string code, string description)
        => new(code, description, ErrorType.Unauthorized);

    public static Error Forbidden(string code, string description)
        => new(code, description, ErrorType.Forbidden);

    public static Error NotFound(string code, string description)
        => new(code, description, ErrorType.NotFound);

    public static Error Conflict(string code, string description)
        => new(code, description, ErrorType.Conflict);
}

public enum ErrorType
{
    Validation,
    Failure,
    Unauthorized,
    Forbidden,
    NotFound,
    Conflict
}
