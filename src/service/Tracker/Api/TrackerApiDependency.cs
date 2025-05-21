using System;
using GarageGroup.Infra;
using PrimeFuncPack;

namespace GGroupp.Yandex.Migration;

public static class TrackerApiDependency
{
    public static Dependency<ITrackerApi> UseTrackerApi(this Dependency<IHttpApi> dependency)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Map<ITrackerApi>(CreateApi);

        static TrackerApi CreateApi(IHttpApi httpApi)
        {
            ArgumentNullException.ThrowIfNull(httpApi);
            return new(httpApi);
        }
    }
}