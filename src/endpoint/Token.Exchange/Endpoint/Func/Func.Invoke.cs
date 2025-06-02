using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;

namespace GGroupp.Yandex.Migration;

partial class TokenExchangeFunc
{
    public ValueTask<Result<TokenExchangeOut, Failure<TokenExchangeFailureCode>>> InvokeAsync(
        TokenExchangeIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            Validate)
        .MapSuccess(
            static @in => new TokenExchangeRequestJson
            {
                YandexPassportOauthToken = @in.YandexPassportOauthToken,
            })
        .Pipe(
            static @in => new HttpSendIn(HttpVerb.Post, $"/iam/v1/tokens")
            {
                Body = HttpBody.SerializeAsJson(@in.SuccessOrThrow()) //TODO: i dont get it how to properly handle this case
            })
        .PipeValue(
            httpApi.SendAsync)
        .Map(
            static success => success.Body.DeserializeFromJson<TokenExchangeResponseJson>(),
            ReadTokenFailure)
        .Map(
            static success => new TokenExchangeOut
            {
                IamToken = success.IamToken,
                ExpiresAt = success.ExpiresAt
            },
            static failure => failure.MapFailureCode(MapTokenExchangeFailureCode));
}