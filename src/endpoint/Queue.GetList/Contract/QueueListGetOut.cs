using System;
using GarageGroup.Infra;

namespace GGroupp.Yandex.Migration;

public readonly record struct QueueListGetOut
{
    [RootBodyOut]
    public FlatArray<QueueItem> Queues { get; init; }
}
