using EasyPOS.Domain.Shared;

namespace EasyPOS.WebApi.Extensions;

public static class ResultExtensions
{
    public static IResult ToProblemDetails(this Result result)
    {
        if (result.IsSuccess) throw new InvalidOperationException();

        if (result is IValidationResult validationResult)
        {
            return Results.Problem(
                statusCode: GetStatusCode(result.Error.ErrorType),
                title: "Validation Error",
                type: GetType(result.Error.ErrorType),
                detail: result.Error.Description,
                extensions: new Dictionary<string, object?>
                {
                      {"errors", validationResult.Errors }
                });
        }

        return Results.Problem(
            statusCode: GetStatusCode(result.Error.ErrorType),
            title: GetTitle(result.Error.ErrorType),
            type: GetType(result.Error.ErrorType),
            extensions: new Dictionary<string, object?>
            {
                {"errors", new[] { result.Error } }
            });
    }

    static int GetStatusCode(ErrorType errorType) =>
        errorType switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

    static string GetTitle(ErrorType errorType) =>
     errorType switch
     {
         ErrorType.Validation => "Bad Request",
         ErrorType.Unauthorized => "Unauthorized",
         ErrorType.Forbidden => "Forbidden",
         ErrorType.NotFound => "Not Found",
         ErrorType.Conflict => "Conflict",
         _ => "Internal Server Error"
     };

    static string GetType(ErrorType errorType) =>
        errorType switch
        {
            ErrorType.Validation => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            ErrorType.Unauthorized => "https://tools.ietf.org/html/rfc7235#section-3.1",
            ErrorType.Forbidden => "https://tools.ietf.org/html/rfc7231#section-6.5.3",
            ErrorType.NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            ErrorType.Conflict => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
            _ => "https://tools.ietf.org/html/rfc7231#section-6.6.1"
        };

}
