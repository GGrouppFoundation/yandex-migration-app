using System;
using GarageGroup;

namespace GGroupp.Yandex.Migration;

internal sealed partial class ConfigurationExportFunc(IQueueGetSupplier trackerQueueApi, IUserGetSupplier trackerUserApi) : IConfigurationExportFunc
{
    private const string FileExtension = ".ytexp";

    private const string FileNamePrefix = "tracker-config-";

    private static readonly PipelineParallelOption ParallelOption
        =
        new()
        {
            DegreeOfParallelism = 4,
            FailureAction = PipelineParallelFailureAction.Stop
        };

    private static Result<ConfigurationExportIn, Failure<ConfigurationExportFailureCode>> Validate(ConfigurationExportIn input)
    {
        if (string.IsNullOrWhiteSpace(input.OrganizationId))
        {
            return Failure.Create(ConfigurationExportFailureCode.EmptyOrganizationId, "Organization ID is empty.");
        }

        if (input.QueueIds.IsEmpty)
        {
            return Failure.Create(ConfigurationExportFailureCode.EmptyQueueIds, "At least one QueueId must be specified.");
        }

        return input;
    }

    private static FlatArray<TrackerQueueGetIn> ExtractTrackerRequests(ConfigurationExportIn input)
    {
        return input.QueueIds.Map(InnerMap);

        TrackerQueueGetIn InnerMap(int queueId)
            =>
            new()
            {
                OrganizationId = input.OrganizationId,
                QueueId = queueId
            };
    }

    private static IssueTypeConfigExport MapIssueTypeConfig(TrackerQueueGetOut.QueueIssueTypeConfig queueIssueTypeConfig)
        =>
        new()
        {
            IssueType = queueIssueTypeConfig.IssueType.Key,
            Workflow = queueIssueTypeConfig.Workflow.Id,
            Resolutions = queueIssueTypeConfig.Resolutions.Map(MapResolution)
        };

    private static string MapResolution(TrackerQueueGetOut.QueueResolution queueResolution)
        =>
        queueResolution.Key;

    private static ConfigurationExportFailureCode MapQueueFailureCode(TrackerQueueGetFailureCode failureCode)
        =>
        failureCode switch
        {
            TrackerQueueGetFailureCode.Forbidden => ConfigurationExportFailureCode.Forbidden,
            TrackerQueueGetFailureCode.NotFound => ConfigurationExportFailureCode.QueueNotFound,
            _ => ConfigurationExportFailureCode.Unknown
        };

    private static ConfigurationExportFailureCode MapUserFailureCode(TrackerUserGetFailureCode failureCode)
        =>
        failureCode switch
        {
            TrackerUserGetFailureCode.Forbidden => ConfigurationExportFailureCode.Forbidden,
            _ => ConfigurationExportFailureCode.Unknown
        };

    private sealed record QueueExportData
    {
        public required string Key { get; init; }

        public required string Name { get; init; }

        public required string Lead { get; init; }

        public required string DefaultType { get; init; }

        public required string DefaultPriority { get; init; }

        public required FlatArray<IssueTypeConfigExport> IssueTypesConfig { get; init; }
    }

    private sealed record IssueTypeConfigExport
    {
        public required string IssueType { get; init; }

        public required string Workflow { get; init; }

        public required FlatArray<string> Resolutions { get; init; }
    }
}
