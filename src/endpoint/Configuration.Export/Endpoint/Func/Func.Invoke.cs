using System;
using System.IO;
using System.IO.Compression;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup;

namespace GGroupp.Yandex.Migration;

partial class ConfigurationExportFunc
{
    public ValueTask<Result<ConfigurationExportOut, Failure<ConfigurationExportFailureCode>>> InvokeAsync(
        ConfigurationExportIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            Validate)
        .MapSuccess(
            ExtractTrackerRequests)
        .ForwardParallelValue(
            GetQueueExportDataAsync,
            ParallelOption)
        .Forward(
            PackIntoFile)
        .MapSuccess(
            static file => new ConfigurationExportOut(
                fileName: $"tracker-{DateTime.Now:yyyyMMddHHmmss}.zip",
                file: file));

    private ValueTask<Result<QueueExportData, Failure<ConfigurationExportFailureCode>>> GetQueueExportDataAsync(
        TrackerQueueGetIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .PipeValue(
            trackerQueueApi.GetQueueAsync)
        .MapFailure(
            static failure => failure.MapFailureCode(MapQueueFailureCode))
        .ForwardValue(
            (queue, token) => PrepareExportDataAsync(input, queue, token));

    private ValueTask<Result<QueueExportData, Failure<ConfigurationExportFailureCode>>> PrepareExportDataAsync(
        TrackerQueueGetIn input, TrackerQueueGetOut queue, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            queue, cancellationToken)
        .Pipe(
            queue => new TrackerUserGetIn
            {
                OrganizationId = input.OrganizationId,
                UserId = queue.Lead.Id
            })
        .PipeValue(
            trackerUserApi.GetUserAsync)
        .MapFailure(
            static failure => failure.MapFailureCode(MapUserFailureCode))
        .MapSuccess(
            user => new QueueExportData
            {
                Key = queue.Key,
                Name = queue.Name,
                LeadLogin = user.Login,
                DefaultType = queue.DefaultType.Key,
                DefaultPriority = queue.DefaultPriority.Key,
                IssueTypesConfig = queue.IssueTypesConfig.Map(config => MapIssueTypeConfig(config))
            });

    private static Result<MemoryStream, Failure<ConfigurationExportFailureCode>> PackIntoFile(FlatArray<QueueExportData> queues)
    {
        try
        {
            var memoryStream = new MemoryStream();

            using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                foreach (var queue in queues)
                {
                    var json = JsonSerializer.Serialize(queue, JsonSerializerOptions.Web);
                    var entry = zipArchive.CreateEntry($"{queue.Key}/{queue.Key}.json");
                    using var entryStream = entry.Open();
                    using var writer = new StreamWriter(entryStream);
                    writer.Write(json);
                }
            }

            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        catch (Exception ex)
        {
            return ex.ToFailure(ConfigurationExportFailureCode.Unknown, $"Failed to create the ZIP archive: {ex.Message}");
        }
    }
}