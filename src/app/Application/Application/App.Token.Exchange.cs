using GarageGroup.Infra;
using PrimeFuncPack;

namespace GGroupp.Yandex.Migration;

partial class Application
{
    [EndpointApplicationExtension]
    internal static Dependency<TokenExchangeEndpoint> UseTokenExchangeEndpoint()
        =>
        UseTokenApi().UseTokenExchangeEndpoint();
}