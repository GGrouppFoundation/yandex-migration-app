using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;

namespace GGroupp.Yandex.Migration;

partial class TrackerApi
{
    public ValueTask<Result<TrackerOrganizationListGetOut, Failure<TrackerOrganizationListGetFailureCode>>> GetOrganizationsAsync(
        Unit input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            static _ => new HttpSendIn(HttpVerb.Get, OrganizationsUrl))
        .PipeValue(
            httpApi.SendAsync)
        .Map(
            static success => success.Body.DeserializeFromJson<OrganizationListJson>(),
            ReadOrganizationFailure)
        .Map(
            static list => new TrackerOrganizationListGetOut
            {
                Organizations = list.Organizations.Map(MapOrganization)
            },
            static failure => failure.MapFailureCode(MapOrganizationListGetFailureCode));

    private static TrackerOrganization MapOrganization(OrganizationJson organization)
        =>
        new()
        {
            Id = organization.Id.OrEmpty(),
            Title = organization.Title.OrEmpty()
        };

    private static Failure<HttpFailureCode> ReadOrganizationFailure(HttpSendFailure failure)
    {
        if (failure.Body.Type.IsJsonMediaType(isApplicationJsonStrict: false))
        {
            var message = failure.Body.DeserializeFromJson<OrganizationFailureJson>().Message;
            if (string.IsNullOrWhiteSpace(message) is false)
            {
                return new(failure.StatusCode, message);
            }
        }

        return failure.ToStandardFailure("An unexpected error occured when trying to get organizations:");
    }

    private static TrackerOrganizationListGetFailureCode MapOrganizationListGetFailureCode(HttpFailureCode failureCode)
        =>
        failureCode switch
        {
            HttpFailureCode.Forbidden => TrackerOrganizationListGetFailureCode.Forbidden,
            HttpFailureCode.Unauthorized => TrackerOrganizationListGetFailureCode.Unauthorized,
            _ => default
        };
}