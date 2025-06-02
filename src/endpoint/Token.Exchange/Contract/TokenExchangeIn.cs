using System;
using GarageGroup.Infra;

namespace GGroupp.Yandex.Migration;

public sealed record class TokenExchangeIn
{
    public TokenExchangeIn(
        [JsonBodyIn] string yandexPassportOauthToken)
    {
        YandexPassportOauthToken = yandexPassportOauthToken.OrEmpty();
    }

    public string YandexPassportOauthToken { get; }
}