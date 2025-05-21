using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Yandex.Migration;

partial class OrganizationListGetFunc
{
    public ValueTask<Result<OrganizationListGetOut, Failure<OrganizationListGetFailureCode>>> InvokeAsync(
        Unit input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .PipeValue(
            trackerApi.GetOrganizationsAsync)
        .Map(
            static success => new OrganizationListGetOut
            {
                Organizations = success.Organizations.Map(MapOrganization)
            },
            static failure => failure.MapFailureCode(MapFailureCode));

    private static OrganizationItem MapOrganization(TrackerOrganization organization)
        =>
        new()
        {
            Id = organization.Id,
            Title = organization.Title
        };

    private static OrganizationListGetFailureCode MapFailureCode(TrackerOrganizationListGetFailureCode failureCode)
        =>
        failureCode switch
        {
            TrackerOrganizationListGetFailureCode.Forbidden => OrganizationListGetFailureCode.Forbidden,
            _ => default
        };
}