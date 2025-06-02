using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace GGroupp.Yandex.Migration;

static class Program
{
    static Task Main(string[] args)
        =>
        AzureApplication.Create(args, ApplicationHost.Configure)
        .AllowCors()
        .UseHealthCheck()
        .UseSwagger()
        .UseStandardSwaggerUI()
        .UseTokenExchangeEndpoint()
        .UseTokenReader()
        .UseOrganizationListGetEndpoint()
        .UseQueueListGetEndpoint()
        .UseConfigurationExportEndpoint()
        .UseConfigurationImportEndpoint()
        .RunAsync();
}