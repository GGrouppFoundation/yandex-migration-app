using GarageGroup.Infra;

namespace GGroupp.Yandex.Migration;

public enum ConfigurationImportFailureCode
{
    Unknown,

    [Problem(FailureStatusCode.BadRequest, "OrganizationId must be specified.")]
    EmptyOrganizationId,

    [Problem(FailureStatusCode.BadRequest, "A file must be provided for import.")]
    FileNotProvided,

    [Problem(FailureStatusCode.BadRequest, "The uploaded file is empty.")]
    FileIsEmpty,

    [Problem(FailureStatusCode.BadRequest, "Invalid file format or extension. Only .ytexp files are allowed.")]
    InvalidFileFormatOrExtension,

    [Problem(FailureStatusCode.NotFound, detailFromFailureMessage: true)]
    ReferenceNotFound,

    [Problem(FailureStatusCode.BadRequest, detailFromFailureMessage: true)]
    QueueCreationFailure,

    [Problem(FailureStatusCode.Conflict, detailFromFailureMessage: true)]
    QueueConflictCreationFailure,

    [Problem(FailureStatusCode.UnprocessableEntity, detailFromFailureMessage: true)]
    QueueValidationFailure,

    [Problem(FailureStatusCode.Forbidden, detailFromFailureMessage: true)]
    Forbidden
}