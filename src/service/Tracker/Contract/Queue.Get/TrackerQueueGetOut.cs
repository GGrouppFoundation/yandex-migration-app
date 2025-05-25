using System;

namespace GGroupp.Yandex.Migration;

public sealed record class TrackerQueueGetOut
{
    public required string Key { get; init; }

    public required string Name { get; init; }

    public required QueueLead Lead { get; init; }

    public required QueueDefaultType DefaultType { get; init; }

    public required QueueDefaultPriority DefaultPriority { get; init; }

    public required FlatArray<QueueIssueTypeConfig> IssueTypesConfig { get; init; }

    public sealed record class QueueLead
    {
        public required string Id { get; init; }
    }

    public sealed record class QueueDefaultType
    {
        public required string Key { get; init; }
    }

    public sealed record class QueueDefaultPriority
    {
        public required string Key { get; init; }
    }

    public sealed record class QueueIssueTypeConfig
    {
        public required QueueIssueType IssueType { get; init; }

        public required QueueWorkflow Workflow { get; init; }

        public FlatArray<QueueResolution> Resolutions { get; init; }
    }

    public sealed record class QueueIssueType
    {
        public required string Key { get; init; }
    }

    public sealed record class QueueWorkflow
    {
        public required string Id { get; init; }
    }

    public sealed record class QueueResolution
    {
        public required string Key { get; init; }
    }
}