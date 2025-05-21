namespace GGroupp.Yandex.Migration;

public sealed record class TrackerQueue
{
    public required string Id { get; init; }

    public required string Key { get; init; }

    public required string Name { get; init; }
}