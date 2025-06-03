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
        .MapSuccess(
            static @in => new HttpSendIn(HttpVerb.Post, "/iam/v1/tokens")
            {
                Body = HttpBody.SerializeAsJson(@in)
            })
        .ForwardValue(
            httpApi.SendAsync,
            static failure => ReadTokenFailure(failure).MapFailureCode(MapTokenExchangeFailureCode))
        .MapSuccess(
            static success => success.Body.DeserializeFromJson<TokenExchangeResponseJson>())
        .MapSuccess(
            static success => new TokenExchangeOut
            {
                IamToken = success.IamToken.OrEmpty(),
                ExpiresAt = success.ExpiresAt.GetValueOrDefault()
            });
}