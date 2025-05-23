using GarageGroup.Infra;

namespace GGroupp.Yandex.Migration;

public enum ConfigurationExportFailureCode
{
    Unknown,

    [Problem(FailureStatusCode.BadRequest, "OrganizationId is required.")]
    EmptyOrganizationId,

    [Problem(FailureStatusCode.BadRequest, "At least one QueueId must be specified.")]
    EmptyQueueIds,

    [Problem(FailureStatusCode.UnprocessableEntity, detailFromFailureMessage: true)]
    QueueProcessingFailure,

    [Problem(FailureStatusCode.InternalServerError, "Failed to create the ZIP archive.")]
    ArchiveCreationFailure,

    [Problem(FailureStatusCode.Forbidden, detailFromFailureMessage: true)]
    Forbidden
}