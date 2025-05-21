using GarageGroup.Infra;
using PrimeFuncPack;

namespace GGroupp.Yandex.Migration;

partial class Application
{
    [EndpointApplicationExtension]
    internal static Dependency<OrganizationListGetEndpoint> UseOrganizationListGetEndpoint()
        =>
        UseTrackerApi().UseOrganizationListGetEndpoint();
}