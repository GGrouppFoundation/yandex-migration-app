using System;
using PrimeFuncPack;

namespace GGroupp.Yandex.Migration;

public static class QueueListGetDependency
{
    public static Dependency<QueueListGetEndpoint> UseQueueListGetEndpoint<TTrackerApi>(this Dependency<TTrackerApi> dependency)
        where TTrackerApi : IQueueListGetSupplier
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Map(CreateFunc).Map(QueueListGetEndpoint.Resolve);

        static QueueListGetFunc CreateFunc(TTrackerApi trackerApi)
        {
            ArgumentNullException.ThrowIfNull(trackerApi);
            return new(trackerApi);
        }
    }
}
