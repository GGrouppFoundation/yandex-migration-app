using System;
using GarageGroup;

namespace GGroupp.Yandex.Migration;

internal sealed partial class ConfigurationImportFunc(IQueueCreateSupplier trackerApi) : IConfigurationImportFunc
{
    private static readonly PipelineParallelOption ParallelOption
        =
        new()
        {
            DegreeOfParallelism = 4,
            FailureAction = PipelineParallelFailureAction.Stop
        };

    private static Result<ConfigurationImportIn, Failure<ConfigurationImportFailureCode>> Validate(ConfigurationImportIn input)
    {
        if (string.IsNullOrWhiteSpace(input.OrganizationId))
        {
            return Failure.Create(ConfigurationImportFailureCode.EmptyOrganizationId, "Organization ID is empty.");
        }

        if (input.File is null)
        {
            return Failure.Create(ConfigurationImportFailureCode.FileNotProvided, "A file must be provided for import.");
        }

        return input;
    }

    private static ConfigurationImportFailureCode MapQueueFailureCode(TrackerQueueCreateFailureCode failureCode)
        =>
        failureCode switch
        {
            TrackerQueueCreateFailureCode.Forbidden => ConfigurationImportFailureCode.Forbidden,
            TrackerQueueCreateFailureCode.Unauthorized => ConfigurationImportFailureCode.Unauthorized,
            _ => ConfigurationImportFailureCode.Unknown
        };

    private static TrackerQueueCreateIn MapTrackerQueue(
        QueueImportData queue, string organizationId)
        =>
        new()
        {
            OrganizationId = organizationId,
            Queue = new()
            {
                Key = queue.Key,
                Name = queue.Name,
                Lead = queue.Lead,
                DefaultType = queue.DefaultType,
                DefaultPriority = queue.DefaultPriority,
                IssueTypesConfig = queue.IssueTypesConfig.Map(MapIssueTypeConfig)
            }
        };

    private static TrackerQueueCreateIn.IssueTypeConfig MapIssueTypeConfig(
        IssueTypeConfig issueTypeConfig)
        =>
        new()
        {
            IssueType = issueTypeConfig.IssueType,
            Workflow = issueTypeConfig.Workflow,
            Resolutions = issueTypeConfig.Resolutions
        };

    private readonly record struct QueueImportData
    {
        public required string Key { get; init; }

        public required string Name { get; init; }

        public required string Lead { get; init; }

        public required string DefaultType { get; init; }

        public required string DefaultPriority { get; init; }

        public FlatArray<IssueTypeConfig> IssueTypesConfig { get; init; }
    }

    private readonly record struct IssueTypeConfig
    {
        public required string IssueType { get; init; }

        public required string Workflow { get; init; }

        public FlatArray<string>? Resolutions { get; init; }
    }
}
