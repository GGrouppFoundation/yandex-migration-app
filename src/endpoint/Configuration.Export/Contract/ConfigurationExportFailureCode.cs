using GarageGroup.Infra;

namespace GGroupp.Yandex.Migration;

public enum ConfigurationExportFailureCode
{
    Unknown,

    [Problem(FailureStatusCode.BadRequest, "OrganizationId is required.")]
    EmptyOrganizationId,

    [Problem(FailureStatusCode.Forbidden, detailFromFailureMessage: true)]
    Forbidden
}