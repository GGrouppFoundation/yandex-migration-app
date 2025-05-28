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
        .ForwardValue(
            ExtractQueueDataToImportAsync)
        .ForwardParallelValue(
            CreateQueueInTrackerAsync,
            ParallelOption)
        .MapSuccess(
            static success => new ConfigurationImportOut
            {
                ImportResults = success
            });

    private static async ValueTask<Result<FlatArray<TrackerQueueCreateIn>, Failure<ConfigurationImportFailureCode>>> ExtractQueueDataToImportAsync(
        ConfigurationImportIn input, CancellationToken cancellationToken)
    {
        try
        {
            using var memoryStream = new MemoryStream();
            await input.File.CopyToAsync(memoryStream, cancellationToken);

            if (memoryStream.Length is 0)
            {
                return Failure.Create(ConfigurationImportFailureCode.ZipIsEmpty, "The uploaded file is empty.");
            }

            memoryStream.Position = 0;

            using var archive = new ZipArchive(memoryStream, ZipArchiveMode.Read, false);
            var queues = new List<TrackerQueueCreateIn>(archive.Entries.Count);

            foreach (ZipArchiveEntry entry in archive.Entries)
            {
                var proccesedEntry = await ProcessEntryAsync(entry, input.OrganizationId, cancellationToken);

                if (proccesedEntry.IsFailure)
                {
                    return proccesedEntry.FailureOrThrow();
                }

                queues.Add(proccesedEntry.SuccessOrThrow());
            }

            return queues.ToFlatArray();
        }
        catch (InvalidDataException ex)
        {
            return ex.ToFailure(ConfigurationImportFailureCode.ZipIsInvalid, $"The uploaded file is not a valid ZIP archive or is corrupted: {ex.Message}");
        }
        catch (Exception ex)
        {
            return ex.ToFailure(ConfigurationImportFailureCode.Unknown, $"An unexpected error occurred while processing the file: {ex.Message}");
        }
    }

    private ValueTask<Result<ImportResult, Failure<ConfigurationImportFailureCode>>> CreateQueueInTrackerAsync(
       TrackerQueueCreateIn queue, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            queue, cancellationToken)
        .PipeValue(
            trackerApi.CreateQueueAsync)
        .MapSuccess(
            success => new ImportResult
            {
                OriginalKey = queue.Queue.Key,
                IsSuccess = true,
                NewId = success.Id,
                NewKey = success.Key,
                NewName = success.Name
            })
        .Recover(
            failure => failure.FailureCode switch
            {
                TrackerQueueCreateFailureCode.BadRequest or TrackerQueueCreateFailureCode.Conflict or TrackerQueueCreateFailureCode.ReferenceNotFound => new ImportResult
                {
                    OriginalKey = queue.Queue.Key,
                    IsSuccess = false,
                    FailureReason = failure.FailureCode.ToString(),
                    FailureDetails = failure.FailureMessage
                },
                _ => failure
            })
        .MapFailure(
            static failure => failure.MapFailureCode(MapQueueFailureCode));

    private static async ValueTask<Result<TrackerQueueCreateIn, Failure<ConfigurationImportFailureCode>>> ProcessEntryAsync(
       ZipArchiveEntry entry, string organizationId, CancellationToken cancellationToken)
    {
        try
        {
            using var stream = entry.Open();
            using var reader = new StreamReader(stream);
            var jsonContent = await reader.ReadToEndAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(jsonContent))
            {
                return Failure.Create(ConfigurationImportFailureCode.FileIsEmpty, $"No JSON content in file {entry.FullName}.");
            }

            var queueData = JsonSerializer.Deserialize<QueueImportData>(jsonContent, JsonSerializerOptions.Web);

            return MapTrackerQueue(queueData, organizationId);
        }
        catch (JsonException ex)
        {
            return ex.ToFailure(ConfigurationImportFailureCode.FileIsInvalid, $"Invalid JSON content in file {entry.FullName}.");
        }
    }
}