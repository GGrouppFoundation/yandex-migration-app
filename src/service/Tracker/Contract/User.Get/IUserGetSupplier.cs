using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Yandex.Migration;

public interface IUserGetSupplier
{
    ValueTask<Result<TrackerUserGetOut, Failure<TrackerUserGetFailureCode>>> GetUserAsync(
        TrackerUserGetIn input, CancellationToken cancellationToken);
}