using System;
using GarageGroup.Infra;

namespace GGroupp.Yandex.Migration;

public sealed record class TokenExchangeOut
{
    [JsonBodyOut]
    public required string IamToken { get; init; }

    [JsonBodyOut]
    public DateTime ExpiresAt { get; init; }
}
