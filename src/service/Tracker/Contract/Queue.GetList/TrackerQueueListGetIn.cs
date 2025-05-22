namespace GGroupp.Yandex.Migration;

public sealed record class TrackerQueueListGetIn
{
    public required string OrganizationId { get; init; }
}