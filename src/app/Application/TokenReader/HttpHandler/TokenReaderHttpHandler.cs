using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

namespace GGroupp.Yandex.Migration;

internal sealed partial class TokenReaderHttpHandler : DelegatingHandler
{
    internal static TokenReaderHttpHandler InternalResolve(IServiceProvider serviceProvider, HttpMessageHandler innerHandler)
        =>
        new(
            innerHandler: innerHandler,
            tokenStorage: serviceProvider.GetRequiredService<ITokenStorage>());

    private readonly ITokenGetSupplier tokenStorage;

    private TokenReaderHttpHandler(HttpMessageHandler innerHandler, ITokenGetSupplier tokenStorage) : base(innerHandler)
        =>
        this.tokenStorage = tokenStorage;
}