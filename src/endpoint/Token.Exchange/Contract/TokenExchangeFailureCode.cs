using GarageGroup.Infra;

namespace GGroupp.Yandex.Migration;

public enum TokenExchangeFailureCode
{
    Unknown,

    [Problem(FailureStatusCode.BadRequest, "Token is required.")]
    EmptyToken,

    [Problem(FailureStatusCode.BadRequest, detailFromFailureMessage: true)]
    TokenIsInvalid,

    [Problem(FailureStatusCode.Unauthorized, "OAuth token is invalid or expired.")]
    Unauthorized,
}
