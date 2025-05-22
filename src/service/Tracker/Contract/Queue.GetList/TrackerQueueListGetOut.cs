using System;

namespace GGroupp.Yandex.Migration;

public readonly record struct TrackerQueueListGetOut
{
    public FlatArray<TrackerQueue> Queues { get; init; }
}