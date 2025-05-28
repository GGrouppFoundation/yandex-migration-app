using GarageGroup.Infra;

namespace GGroupp.Yandex.Migration;

public enum ConfigurationExportFailureCode
{
    Unknown,

    [Problem(FailureStatusCode.BadRequest, "OrganizationId is required.")]
    EmptyOrganizationId,

    [Problem(FailureStatusCode.Unauthorized, detailFromFailureMessage: true)]
    Unauthorized,

    [Problem(FailureStatusCode.BadRequest, "At least one QueueId must be specified.")]
    EmptyQueueIds,

    [Problem(FailureStatusCode.NotFound, detailFromFailureMessage: true)]
    QueueNotFound,

    [Problem(FailureStatusCode.Forbidden, detailFromFailureMessage: true)]
    Forbidden
}