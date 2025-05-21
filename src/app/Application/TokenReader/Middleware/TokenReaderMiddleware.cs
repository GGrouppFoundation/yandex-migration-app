using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GGroupp.Yandex.Migration;

internal static partial class TokenReaderMiddleware
{
    private const string BearerSchemaName = "Bearer";

    private const string SecuritySchemeKey = "jwtAuthorization";

    private static readonly string UnauthorizedResponseCode
        =
        StatusCodes.Status401Unauthorized.ToString();

    private const string UnauthorizedResponseDescription = "Unauthorized";

    private static Result<string, Failure<Unit>> GetTokenValue(this HttpContext context)
    {
        var authValue = context.Request.Headers.Authorization.ToString();
        if (string.IsNullOrWhiteSpace(authValue))
        {
            return Failure.Create("Authorization header value must be specified.");
        }

        return authValue;
    }
    
    private static Result<string, Failure<Unit>> GetBearerValue(this HttpContext context, string authHeaderValue)
    {
        var arr = authHeaderValue.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (arr.Length is not 2)
        {
            return Failure.Create($"Authorization header value '{authHeaderValue}' is invalid.");
        }

        if (string.Equals(BearerSchemaName, arr[0], StringComparison.InvariantCultureIgnoreCase) is false)
        {
            return Failure.Create($"Authorization token '{authHeaderValue}' is invalid: the schema must be Bearer.");
        }

        return arr[1];
    }

    private static ILogger? GetLogger(this HttpContext context)
        =>
        context.RequestServices.GetService<ILoggerFactory>()?.CreateLogger("TokenReaderMiddleware");
}