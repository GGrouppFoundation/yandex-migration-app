namespace GGroupp.Yandex.Migration;

public sealed record class TrackerQueueGetIn
{
    public required string OrganizationId { get; init; }

    public required int QueueId { get; init; }
}