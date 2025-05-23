using System;

namespace GGroupp.Yandex.Migration;

internal readonly record struct QueueDetailJson
{
    public string? Key { get; init; }

    public string? Name { get; init; }

    public QueueLeadJson? Lead { get; init; }

    public QueueDefaultTypeJson? DefaultType { get; init; }

    public QueueDefaultPriorityJson? DefaultPriority { get; init; }

    public FlatArray<QueueIssueTypeConfigJson> IssueTypesConfig { get; init; }
}

internal sealed record class QueueLeadJson
{
    public string? Id { get; init; }
}

internal sealed record class QueueDefaultTypeJson
{
    public string? Key { get; init; }
}

internal sealed record class QueueDefaultPriorityJson
{
    public string? Key { get; init; }
}

internal sealed record class QueueIssueTypeConfigJson
{
    public QueueIssueTypeJson? IssueType { get; init; }

    public QueueWorkflowJson? Workflow { get; init; }

    public FlatArray<QueueResolutionJson> Resolutions { get; init; }
}

internal sealed record class QueueIssueTypeJson
{
    public string? Key { get; init; }
}

internal sealed record class QueueWorkflowJson
{
    public string? Id { get; init; }
}

internal sealed record class QueueResolutionJson
{
    public string? Key { get; init; }
}