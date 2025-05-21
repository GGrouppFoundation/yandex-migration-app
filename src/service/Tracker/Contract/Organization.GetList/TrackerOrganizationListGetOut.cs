using System;

namespace GGroupp.Yandex.Migration;

public readonly record struct TrackerOrganizationListGetOut
{
    public FlatArray<TrackerOrganization> Organizations { get; init; }
}