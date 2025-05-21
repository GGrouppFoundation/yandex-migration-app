using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Yandex.Migration;

public interface IOrganizationListGetSupplier
{
    ValueTask<Result<TrackerOrganizationListGetOut, Failure<TrackerOrganizationListGetFailureCode>>> GetOrganizationsAsync(
        Unit input, CancellationToken cancellationToken);
}