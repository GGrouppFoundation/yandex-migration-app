namespace GGroupp.Yandex.Migration;

public sealed record class ImportResult
{
    public required string OriginalKey { get; init; }

    public bool IsSuccess { get; init; }

    public int? NewId { get; init; }

    public string? NewKey { get; init; }

    public string? NewName { get; init; }

    public string? FailureReason { get; init; }

    public string? FailureDetails { get; init; }
}