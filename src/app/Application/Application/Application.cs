using System.Net.Http;
using GarageGroup.Infra;
using PrimeFuncPack;

namespace GGroupp.Yandex.Migration;

internal static partial class Application
{
    private static Dependency<ITrackerApi> UseTrackerApi()
        =>
        PrimaryHandler.UseStandardSocketsHttpHandler()
        .UseLogging("TrackerApi")
        .UsePollyStandard()
        .Map<HttpMessageHandler>(TokenReaderHttpHandler.InternalResolve)
        .UseHttpApi("TrackerApi")
        .UseTrackerApi();

    private static Dependency<IHttpApi> UseTokenApi()
        =>
        PrimaryHandler.UseStandardSocketsHttpHandler()
        .UseLogging("TokenApi")
        .UsePollyStandard()
        .UseHttpApi("TokenApi");
}