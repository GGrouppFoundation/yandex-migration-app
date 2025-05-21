using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;

namespace GGroupp.Yandex.Migration;

[Endpoint(EndpointMethod.Get, "/organizations")]
[EndpointTag("Organization")]
public interface IOrganizationListGetFunc
{
    ValueTask<Result<OrganizationListGetOut, Failure<OrganizationListGetFailureCode>>> InvokeAsync(
        Unit input, CancellationToken cancellationToken);
}