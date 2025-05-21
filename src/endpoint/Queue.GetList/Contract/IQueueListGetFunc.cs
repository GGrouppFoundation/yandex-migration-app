using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;

namespace GGroupp.Yandex.Migration;

[Endpoint(EndpointMethod.Get, "/queues")]
[EndpointTag("Queue")]
public interface IQueueListGetFunc
{
    ValueTask<Result<QueueListGetOut, Failure<QueueListGetFailureCode>>> InvokeAsync(
        QueueListGetIn input, CancellationToken cancellationToken);
}
