using System;

namespace GGroupp.Yandex.Migration;

internal readonly record struct OrganizationListJson
{
    public FlatArray<OrganizationJson> Organizations { get; init; }
}