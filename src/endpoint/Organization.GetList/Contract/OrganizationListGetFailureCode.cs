using GarageGroup.Infra;

namespace GGroupp.Yandex.Migration;

public enum OrganizationListGetFailureCode
{
    Unknown,

    [Problem(FailureStatusCode.Unauthorized, detailFromFailureMessage: true)]
    Unauthorized,

    [Problem(FailureStatusCode.Forbidden, "Not enough permissions.")]
    Forbidden
}