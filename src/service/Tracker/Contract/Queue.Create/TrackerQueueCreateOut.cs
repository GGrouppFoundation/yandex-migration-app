namespace GGroupp.Yandex.Migration;

public sealed record class TrackerQueueCreateOut
{
    public required int Id { get; init; }

    public required string Key { get; init; }

    public required string Name { get; init; }

    public required QueueLead Lead { get; init; }

    public sealed record class QueueLead
    {
        public required string Id { get; init; }

        public required string Display { get; init; }
    }
}