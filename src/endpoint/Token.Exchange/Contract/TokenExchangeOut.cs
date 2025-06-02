using System;
using GarageGroup.Infra;

namespace GGroupp.Yandex.Migration;

public sealed record class TokenExchangeOut
{
    [JsonBodyOut]
    public string IamToken { get; init; } = string.Empty;

    [JsonBodyOut]
    public DateTime ExpiresAt { get; init; }
}
