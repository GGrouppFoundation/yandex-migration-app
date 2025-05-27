using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup;

namespace GGroupp.Yandex.Migration;

partial class ConfigurationImportFunc
{
    public ValueTask<Result<ConfigurationImportOut, Failure<ConfigurationImportFailureCode>>> InvokeAsync(
        ConfigurationImportIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            Validate)
        .MapSuccess(
            ExtractQueueDataToImport)
        .ForwardParallelValue(
            CreateQueuesInTrackerAsync, ParallelOption)
        .MapSuccess(
            static success => new ConfigurationImportOut
            {
                Queues = success.Map(MapQueue)
            });

    private static async Task<FlatArray<TrackerQueueCreateIn>> ExtractQueueDataToImport( // errors check + return Result?
        ConfigurationImportIn input)
    {
        var queues = new List<TrackerQueueCreateIn>();

        using var memoryStream = new MemoryStream();
        await input.File.CopyToAsync(memoryStream);
        memoryStream.Position = 0;

        using var archive = new ZipArchive(memoryStream, ZipArchiveMode.Read, false);

        foreach (ZipArchiveEntry entry in archive.Entries)
        {
            using var stream = entry.Open();
            using var reader = new StreamReader(stream);
            var jsonContent = await reader.ReadToEndAsync();

            if (string.IsNullOrWhiteSpace(jsonContent))
            {
                continue;
            }

            var queueData = JsonSerializer.Deserialize<QueueImportData>(jsonContent, JsonSerializerOptions.Web);

            queues.Add(MapTrackerQueue(queueData, input.OrganizationId));
        }

        return queues.ToFlatArray();
    }

    private ValueTask<Result<TrackerQueueCreateOut, Failure<ConfigurationImportFailureCode>>> CreateQueuesInTrackerAsync(
       TrackerQueueCreateIn queue, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            queue, cancellationToken)
        .PipeValue(
            trackerApi.CreateQueueAsync)
        .Map(
            static success => success,
            static failure => failure.MapFailureCode(MapQueueFailureCode));
}