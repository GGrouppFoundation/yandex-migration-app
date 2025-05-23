namespace GGroupp.Yandex.Migration;

public sealed record class TrackerUserGetIn
{
    public required string OrganizationId { get; init; }

    public required string UserId { get; init; }
}