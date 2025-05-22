using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;

namespace GGroupp.Yandex.Migration;

partial class QueueListGetFunc
{
    public ValueTask<Result<QueueListGetOut, Failure<QueueListGetFailureCode>>> InvokeAsync(
        QueueListGetIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            Validate)
        .MapSuccess(
            static @in => new TrackerQueueListGetIn
            {
                OrganizationId = @in.OrganizationId
            })
        .ForwardValue(
            trackerApi.GetQueuesAsync,
            static failure => failure.MapFailureCode(MapFailureCode))
        .MapSuccess(
            static success => new QueueListGetOut
            {
                Queues = success.Queues.Map(MapQueue)
            });

    private static Result<QueueListGetIn, Failure<QueueListGetFailureCode>> Validate(QueueListGetIn input)
    {
        if (string.IsNullOrWhiteSpace(input.OrganizationId))
        {
            return Failure.Create(QueueListGetFailureCode.EmptyOrganizationId, "Organization ID is empty.");
        }

        return input;
    }

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
            _ => QueueListGetFailureCode.Unknown
        };
}