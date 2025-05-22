using GarageGroup.Infra;

namespace GGroupp.Yandex.Migration;

public enum QueueListGetFailureCode
{
    Unknown,

    [Problem(FailureStatusCode.Forbidden, "Not enough permissions.")]
    Forbidden,

    [Problem(FailureStatusCode.BadRequest, "Organization ID is required.")]
    EmptyOrganizationId
}
