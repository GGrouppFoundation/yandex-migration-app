using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Yandex.Migration;

partial class TokenReaderHttpHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var token = tokenStorage.GetToken();
        if (string.IsNullOrWhiteSpace(token) is false)
        {
            request.Headers.Authorization = new("Bearer", token);
        }

        return base.SendAsync(request, cancellationToken);
    }
}