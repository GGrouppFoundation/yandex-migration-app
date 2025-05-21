using System;
using PrimeFuncPack;

namespace GGroupp.Yandex.Migration;

public static class OrganizationListGetDependency
{
    public static Dependency<OrganizationListGetEndpoint> UseOrganizationListGetEndpoint<TTrackerApi>(this Dependency<TTrackerApi> dependency)
        where TTrackerApi : IOrganizationListGetSupplier
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Map(CreateFunc).Map(OrganizationListGetEndpoint.Resolve);

        static OrganizationListGetFunc CreateFunc(TTrackerApi trackerApi)
        {
            ArgumentNullException.ThrowIfNull(trackerApi);
            return new(trackerApi);
        }
    }
}