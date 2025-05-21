using GGroupp.Yandex.Migration.TokenReader;

namespace GGroupp.Yandex.Migration;

internal interface ITokenStorage : ITokenSaveSupplier, ITokenGetSupplier;