using System;
using System.Collections.Concurrent;
using GarageGroup.Infra;

namespace GGroupp.Yandex.Migration;

internal sealed partial class TrackerApi(IHttpApi httpApi) : ITrackerApi
{
    private const string OrganizationsUrl
        =
        "https://organization-manager.api.cloud.yandex.net/organization-manager/v1/organizations";

    private readonly ConcurrentDictionary<TrackerUserGetIn, TrackerUserGetOut> UserCache
        =
        new();

    private static Failure<HttpFailureCode> ReadTrackerFailure(HttpSendFailure failure)
    {
        if (failure.Body.Type.IsJsonMediaType(isApplicationJsonStrict: false))
        {
            var message = string.Join("; ", failure.Body.DeserializeFromJson<TrackerFailureJson>().ErrorMessages.AsEnumerable());
            if (string.IsNullOrWhiteSpace(message) is false)
            {
                return new(failure.StatusCode, message);
            }
        }

        return failure.ToStandardFailure("An unexpected error occured when trying to create queue:");
    }

    private static FlatArray<System.Collections.Generic.KeyValuePair<string, string>> BuildHeader(string organizationId)
        =>
        [new("X-Cloud-Org-ID", organizationId)];
}