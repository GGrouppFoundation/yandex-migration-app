using System;

namespace GGroupp.Yandex.Migration;

internal readonly record struct QueueDetailJson
{
    public string? Key { get; init; }

    public string? Name { get; init; }

    public LeadJson? Lead { get; init; }

    public DefaultTypeJson? DefaultType { get; init; }

    public DefaultPriorityJson? DefaultPriority { get; init; }

    public FlatArray<IssueTypeConfigJson> IssueTypesConfig { get; init; }

    internal sealed record class LeadJson
    {
        public string? Id { get; init; }
    }

    internal sealed record class DefaultTypeJson
    {
        public string? Key { get; init; }
    }

    internal sealed record class DefaultPriorityJson
    {
        public string? Key { get; init; }
    }

    internal sealed record class IssueTypeConfigJson
    {
        public IssueTypeJson? IssueType { get; init; }

        public WorkflowJson? Workflow { get; init; }

        public FlatArray<ResolutionJson> Resolutions { get; init; }
    }

    internal sealed record class IssueTypeJson
    {
        public string? Key { get; init; }
    }

    internal sealed record class WorkflowJson
    {
        public string? Id { get; init; }
    }

    internal sealed record class ResolutionJson
    {
        public string? Key { get; init; }
    }
}