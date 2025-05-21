using GarageGroup.Infra;

namespace GGroupp.Yandex.Migration;

internal sealed partial class TrackerApi(IHttpApi httpApi) : ITrackerApi
{
    private const string OrganizationsUrl = "https://organization-manager.api.cloud.yandex.net/organization-manager/v1/organizations";
}