using System;

namespace GGroupp.Yandex.Migration;

internal sealed class InMemoryTokenStorage : ITokenStorage
{
    private string? token;

    public Unit SaveToken(string token)
    {
        this.token = token;
        return default;
    }

    public string? GetToken()
        =>
        token;
}
