using System;

namespace GGroupp.Yandex.Migration;

internal readonly record struct TrackerFailureJson
{
    public FlatArray<string> ErrorMessages { get; init; }
}