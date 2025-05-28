using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Yandex.Migration;

public interface IQueueCreateSupplier
{
    ValueTask<Result<TrackerQueueCreateOut, Failure<TrackerQueueCreateFailureCode>>> CreateQueueAsync(
        TrackerQueueCreateIn input, CancellationToken cancellationToken);
}