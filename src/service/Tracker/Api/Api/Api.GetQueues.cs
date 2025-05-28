using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;

namespace GGroupp.Yandex.Migration;

partial class TrackerApi
{
    public ValueTask<Result<TrackerQueueListGetOut, Failure<TrackerQueueListGetFailureCode>>> GetQueuesAsync(
        TrackerQueueListGetIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            static @in => new HttpSendIn(HttpVerb.Get, "/v3/queues")
            {
                Headers = BuildHeader(@in.OrganizationId)
            })
        .PipeValue(
            httpApi.SendAsync)
        .Map(
            static success => success.Body.DeserializeFromJson<FlatArray<QueueJson>>(),
            ReadTrackerFailure)
        .Map(
            static list => new TrackerQueueListGetOut
            {
                Queues = list.Map(MapQueue)
            },
            static failure => failure.MapFailureCode(MapQueueListGetFailureCode));

    private static TrackerQueue MapQueue(QueueJson queue)
        =>
        new()
        {
            Id = queue.Id,
            Key = queue.Key.OrEmpty(),
            Name = queue.Name.OrEmpty()
        };

    private static TrackerQueueListGetFailureCode MapQueueListGetFailureCode(HttpFailureCode failureCode)
        =>
        failureCode switch
        {
            HttpFailureCode.UnprocessableContent => TrackerQueueListGetFailureCode.EmptyOrganizationId,
            HttpFailureCode.Unauthorized => TrackerQueueListGetFailureCode.Unauthorized,
            HttpFailureCode.Forbidden => TrackerQueueListGetFailureCode.Forbidden,
            _ => default
        };
}