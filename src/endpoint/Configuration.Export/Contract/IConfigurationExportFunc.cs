using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;

namespace GGroupp.Yandex.Migration;

[Endpoint(EndpointMethod.Post, "/organizations/{organizationId}/configuration/export")]
[EndpointTag("Configuration")]
public interface IConfigurationExportFunc
{
    ValueTask<Result<ConfigurationExportOut, Failure<ConfigurationExportFailureCode>>> InvokeAsync(
        ConfigurationExportIn input, CancellationToken cancellationToken);
}