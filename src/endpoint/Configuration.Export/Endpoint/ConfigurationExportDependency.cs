using System;
using PrimeFuncPack;

namespace GGroupp.Yandex.Migration;

public static class OrganizationListGetDependency
{
    public static Dependency<ConfigurationExportEndpoint> UseConfigurationExportEndpoint<TTrackerApi>(this Dependency<TTrackerApi> dependency)
        where TTrackerApi : IUserGetSupplier, IQueueGetSupplier
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Map(CreateFunc).Map(ConfigurationExportEndpoint.Resolve);

        static ConfigurationExportFunc CreateFunc(TTrackerApi trackerApi)
        {
            ArgumentNullException.ThrowIfNull(trackerApi);
            return new(trackerApi, trackerApi);
        }
    }
}