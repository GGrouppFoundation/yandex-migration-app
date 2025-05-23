using System;

namespace GGroupp.Yandex.Migration;

public sealed record class TrackerQueueGetOut
{
    public required string Key { get; init; }

    public required string Name { get; init; }

    public required TrackerQueueLead Lead { get; init; }

    public required TrackerQueueDefaultType DefaultType { get; init; }

    public required TrackerQueueDefaultPriority DefaultPriority { get; init; }

    public required FlatArray<TrackerQueueIssueTypeConfig> IssueTypesConfig { get; init; }
}

public sealed record class TrackerQueueLead
{
    public required string Id { get; init; }
}

public sealed record class TrackerQueueDefaultType
{
    public required string Key { get; init; }
}

public sealed record class TrackerQueueDefaultPriority
{
    public required string Key { get; init; }
}

public sealed record class TrackerQueueIssueTypeConfig
{
    public required TrackerQueueIssueType IssueType { get; init; }

    public required TrackerQueueWorkflow Workflow { get; init; }

    public FlatArray<TrackerQueueResolution> Resolutions { get; init; }
}

public sealed record class TrackerQueueIssueType
{
    public required string Key { get; init; }
}

public sealed record class TrackerQueueWorkflow
{
    public required string Id { get; init; }
}

public sealed record class TrackerQueueResolution
{
    public required string Key { get; init; }
}