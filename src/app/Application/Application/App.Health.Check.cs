using GarageGroup.Infra;
using PrimeFuncPack;

namespace GGroupp.Yandex.Migration;

partial class Application
{
    [HandlerApplicationExtension(HttpMethodName.Get, "/health")]
    internal static Dependency<IHealthCheckHandler> UseHealthCheck()
        =>
        HealthCheck.UseEmpty().UseHealthCheckHandler();
}