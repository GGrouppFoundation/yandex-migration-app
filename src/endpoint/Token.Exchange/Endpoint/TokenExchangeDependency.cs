using System;
using GarageGroup.Infra;
using PrimeFuncPack;

namespace GGroupp.Yandex.Migration;

public static class TokenExchangeDependency
{
    public static Dependency<TokenExchangeEndpoint> UseTokenExchangeEndpoint(this Dependency<IHttpApi> dependency)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Map(CreateApi).Map(TokenExchangeEndpoint.Resolve);

        static TokenExchangeFunc CreateApi(IHttpApi httpApi)
        {
            ArgumentNullException.ThrowIfNull(httpApi);
            return new(httpApi);
        }
    }
}
