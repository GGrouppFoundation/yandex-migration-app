using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Yandex.Migration;

public interface IQueueListGetSupplier
{
    ValueTask<Result<TrackerQueueListGetOut, Failure<TrackerQueueListGetFailureCode>>> GetQueuesAsync(
        string input, CancellationToken cancellationToken);
}