using System;
using GarageGroup.Infra;

namespace GGroupp.Yandex.Migration;

internal sealed partial class TokenExchangeFunc(IHttpApi httpApi) : ITokenExchangeFunc
{
    private static Result<TokenExchangeIn, Failure<TokenExchangeFailureCode>> Validate(TokenExchangeIn input)
    {
        if (string.IsNullOrWhiteSpace(input.YandexPassportOauthToken))
        {
            return Failure.Create(TokenExchangeFailureCode.EmptyToken, "Token is required.");
        }

        return input;
    }

    private sealed record class TokenExchangeRequestJson
    {
        public required string YandexPassportOauthToken { get; init; }
    }

    private readonly record struct TokenExchangeResponseJson
    {
        public string? IamToken { get; init; }

        public DateTime? ExpiresAt { get; init; }
    }

    private static Failure<HttpFailureCode> ReadTokenFailure(HttpSendFailure failure)
    {
        if (failure.Body.Type.IsJsonMediaType(isApplicationJsonStrict: false))
        {
            var message = failure.Body.DeserializeFromJson<TokenExchangeFailureJson>().Message;
            if (string.IsNullOrWhiteSpace(message) is false)
            {
                return new(failure.StatusCode, message);
            }
        }

        return failure.ToStandardFailure("An unexpected error occured when trying to exchange token:");
    }

    private readonly record struct TokenExchangeFailureJson
    {
        public string? Message { get; init; }
    }

    private static TokenExchangeFailureCode MapTokenExchangeFailureCode(HttpFailureCode failureCode)
        =>
        failureCode switch
        {
            HttpFailureCode.BadRequest => TokenExchangeFailureCode.TokenIsInvalid,
            HttpFailureCode.Unauthorized => TokenExchangeFailureCode.Unauthorized,
            _ => TokenExchangeFailureCode.Unknown
        };
};