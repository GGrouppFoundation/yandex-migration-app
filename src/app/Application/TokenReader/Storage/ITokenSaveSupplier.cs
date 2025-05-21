using System;

namespace GGroupp.Yandex.Migration.TokenReader;

internal interface ITokenSaveSupplier
{
    Unit SaveToken(string token);
}