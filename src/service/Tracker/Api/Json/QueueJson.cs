namespace GGroupp.Yandex.Migration;

internal sealed record class QueueJson
{
    public int Id { get; init; }

    public string? Key { get; init; }

    public string? Name { get; init; }
}