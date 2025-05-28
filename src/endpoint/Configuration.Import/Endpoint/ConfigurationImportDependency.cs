using System;
using PrimeFuncPack;

namespace GGroupp.Yandex.Migration;

public static class ConfigurationImportDependency
{
    public static Dependency<ConfigurationImportEndpoint> UseConfigurationImportEndpoint<TTrackerApi>(this Dependency<TTrackerApi> dependency)
        where TTrackerApi : IQueueCreateSupplier
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Map(CreateFunc).Map(ConfigurationImportEndpoint.Resolve);

        static ConfigurationImportFunc CreateFunc(TTrackerApi trackerApi)
        {
            ArgumentNullException.ThrowIfNull(trackerApi);
            return new(trackerApi);
        }
    }
}