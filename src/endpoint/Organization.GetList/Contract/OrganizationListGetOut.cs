using System;
using GarageGroup.Infra;

namespace GGroupp.Yandex.Migration;

public readonly record struct OrganizationListGetOut
{
    [RootBodyOut]
    public FlatArray<OrganizationItem> Organizations { get; init; }
}