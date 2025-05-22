namespace GGroupp.Yandex.Migration;

public sealed record class QueueItem
{
    public required int Id { get; init; }

    public required string Key { get; init; }

    public required string Name { get; init; }
}
