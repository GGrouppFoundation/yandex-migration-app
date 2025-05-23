using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Yandex.Migration;

public interface IQueueGetSupplier
{
    ValueTask<Result<TrackerQueueGetOut, Failure<TrackerQueueGetFailureCode>>> GetQueueAsync(
        TrackerQueueGetIn input, CancellationToken cancellationToken);
}