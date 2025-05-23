using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Yandex.Migration;

public interface IQueueDetailGetSupplier
{
    ValueTask<Result<TrackerQueueDetailGetOut, Failure<TrackerQueueDetailGetFailureCode>>> GetQueueDetailAsync(
        TrackerQueueDetailGetIn input, CancellationToken cancellationToken);
}