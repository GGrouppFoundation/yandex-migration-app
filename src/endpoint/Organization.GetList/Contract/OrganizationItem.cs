namespace GGroupp.Yandex.Migration;

public sealed record class OrganizationItem
{
    public required string Id { get; init; }

    public required string Title { get; init; }
}