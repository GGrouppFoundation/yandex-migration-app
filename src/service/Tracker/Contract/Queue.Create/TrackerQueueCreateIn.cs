using System;

namespace GGroupp.Yandex.Migration;

public sealed record class TrackerQueueCreateIn
{
    public required string OrganizationId { get; init; }

    public required QueueItem Queue { get; init; }

    public sealed record class QueueItem
    {
        public required string Key { get; init; }

        public required string Name { get; init; }

        public required string Lead { get; init; }

        public required string DefaultType { get; init; }

        public required string DefaultPriority { get; init; }

        public required FlatArray<IssueTypeConfig> IssueTypesConfig { get; init; }
    }

    public sealed record class IssueTypeConfig
    {
        public required string IssueType { get; init; }

        public required string Workflow { get; init; }

        public FlatArray<string> Resolutions { get; init; }
    }
}