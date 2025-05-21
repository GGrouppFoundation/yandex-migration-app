using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;

namespace GGroupp.Yandex.Migration;

partial class TrackerApi
{
    public ValueTask<Result<TrackerQueueListGetOut, Failure<TrackerQueueListGetFailureCode>>> GetQueuesAsync(
        string input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            static organizationId => new HttpSendIn(HttpVerb.Get, "/v3/queues")
            {
                Headers =
                [
                    new("X-Cloud-Org-ID", organizationId)
                ]
            })
        .PipeValue(
            httpApi.SendAsync)
        .Map(
            static success => success.Body.DeserializeFromJson<FlatArray<QueueJson>>(),
            static failure => failure.ToStandardFailure("An unexpected error occurred when trying to get queues:"))
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
            HttpFailureCode.Forbidden => TrackerQueueListGetFailureCode.Forbidden,
            _ => default
        };
}