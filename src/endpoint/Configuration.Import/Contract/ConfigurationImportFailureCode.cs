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
    ZipIsEmpty,

    [Problem(FailureStatusCode.BadRequest, "Invalid file format.")]
    ZipIsInvalid,

    [Problem(FailureStatusCode.BadRequest, detailFromFailureMessage: true)]
    FileIsEmpty,

    [Problem(FailureStatusCode.BadRequest, detailFromFailureMessage: true)]
    FileIsInvalid,

    [Problem(FailureStatusCode.UnprocessableEntity, detailFromFailureMessage: true)]
    QueueValidationFailure,

    [Problem(FailureStatusCode.Forbidden, detailFromFailureMessage: true)]
    Forbidden,

    [Problem(FailureStatusCode.Unauthorized, detailFromFailureMessage: true)]
    Unauthorized
}