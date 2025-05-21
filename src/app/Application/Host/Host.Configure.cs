using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace GGroupp.Yandex.Migration;

partial class ApplicationHost
{
    internal static void Configure(WebApplicationBuilder builder)
        =>
        builder.Services.AddScoped<ITokenStorage, InMemoryTokenStorage>();
}