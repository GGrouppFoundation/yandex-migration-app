using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;

namespace GGroupp.Yandex.Migration;

[Endpoint(EndpointMethod.Post, "/token")]
[EndpointTag("Token")]
public interface ITokenExchangeFunc
{
    ValueTask<Result<TokenExchangeOut, Failure<TokenExchangeFailureCode>>> InvokeAsync(
        TokenExchangeIn input, CancellationToken cancellationToken);
}
