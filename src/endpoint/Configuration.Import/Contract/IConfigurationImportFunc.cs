using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;

namespace GGroupp.Yandex.Migration;

[Endpoint(EndpointMethod.Post, "/organizations/{organizationId}/configuration/import")]
[EndpointTag("Configuration")]
public interface IConfigurationImportFunc
{
    ValueTask<Result<ConfigurationImportOut, Failure<ConfigurationImportFailureCode>>> InvokeAsync(
        ConfigurationImportIn input, CancellationToken cancellationToken);
}