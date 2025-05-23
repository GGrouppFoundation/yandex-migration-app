using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;

namespace GGroupp.Yandex.Migration;

partial class TrackerApi
{
    public ValueTask<Result<TrackerUserGetOut, Failure<TrackerUserGetFailureCode>>> GetUserAsync(
        TrackerUserGetIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            static @in => new HttpSendIn(HttpVerb.Get, $"/v3/users/{@in.UserId}")
            {
                Headers =
                [
                    new ("X-Cloud-Org-ID", @in.OrganizationId)
                ]
            })
        .PipeValue(
            httpApi.SendAsync)
        .Map(
            static success => success.Body.DeserializeFromJson<UserJson>(),
            static failure => failure.ToStandardFailure("Yandex Tracker API call to get user detail failed.")
        )
        .Map(
            static user => new TrackerUserGetOut
            {
                Login = user.Login.OrEmpty()
            },
            static failure => failure.MapFailureCode(MapUserGetFailureCode));

    private static TrackerUserGetFailureCode MapUserGetFailureCode(HttpFailureCode httpFailureCode)
        =>
        httpFailureCode switch
        {
            HttpFailureCode.NotFound => TrackerUserGetFailureCode.NotFound,
            HttpFailureCode.Forbidden => TrackerUserGetFailureCode.Forbidden,
            HttpFailureCode.Unauthorized => TrackerUserGetFailureCode.Unauthorized,
            _ => TrackerUserGetFailureCode.Unknown
        };
}