using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Yandex.Migration;

partial class QueueListGetFunc
{
    public ValueTask<Result<QueueListGetOut, Failure<QueueListGetFailureCode>>> InvokeAsync(
        QueueListGetIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input.OrganizationId, cancellationToken)
        .PipeValue(
            trackerApi.GetQueuesAsync)
        .Map(
            static success => new QueueListGetOut
            {
                Queues = success.Queues.Map(MapQueue)
            },
            static failure => failure.MapFailureCode(MapFailureCode));

    private static QueueItem MapQueue(TrackerQueue queue)
        =>
        new()
        {
            Id = queue.Id,
            Key = queue.Key,
            Name = queue.Name
        };

    private static QueueListGetFailureCode MapFailureCode(TrackerQueueListGetFailureCode failureCode)
        =>
        failureCode switch
        {
            TrackerQueueListGetFailureCode.Forbidden => QueueListGetFailureCode.Forbidden,
            TrackerQueueListGetFailureCode.EmptyOrganizationId => QueueListGetFailureCode.EmptyOrganizationId,
            _ => QueueListGetFailureCode.Unknown
        };
}