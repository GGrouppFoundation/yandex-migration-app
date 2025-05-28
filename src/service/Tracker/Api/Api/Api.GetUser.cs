using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;

namespace GGroupp.Yandex.Migration;

partial class TrackerApi
{
    public ValueTask<Result<TrackerUserGetOut, Failure<TrackerUserGetFailureCode>>> GetUserAsync(
        TrackerUserGetIn input, CancellationToken cancellationToken)
    {
        if (UserCache.TryGetValue(input, out var user))
        {
            return new(result: user);
        }

        return InnerGetUserAsync(input, cancellationToken);
    }

    private ValueTask<Result<TrackerUserGetOut, Failure<TrackerUserGetFailureCode>>> InnerGetUserAsync(
        TrackerUserGetIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            static @in => new HttpSendIn(HttpVerb.Get, $"/v3/users/{@in.UserId}")
            {
                Headers = BuildHeader(@in.OrganizationId)
            })
        .PipeValue(
            httpApi.SendAsync)
        .Map(
            static success => success.Body.DeserializeFromJson<UserJson>(),
            ReadTrackerFailure)
        .Map(
            static user => new TrackerUserGetOut
            {
                Login = user.Login.OrEmpty()
            },
            static failure => failure.MapFailureCode(MapUserGetFailureCode))
        .OnSuccess(
            @out => UpdateUserCache(input, @out));

    private void UpdateUserCache(TrackerUserGetIn input, TrackerUserGetOut output)
    {
        _ = UserCache.AddOrUpdate(input, output, InnerUpdate);

        TrackerUserGetOut InnerUpdate(TrackerUserGetIn @in, TrackerUserGetOut @out)
            =>
            output;
    }

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