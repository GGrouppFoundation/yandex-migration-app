using System;
using GarageGroup.Infra;

namespace GGroupp.Yandex.Migration;

public sealed record class ConfigurationExportIn
{
    public ConfigurationExportIn(
        [JsonBodyIn] string organizationId,
        [JsonBodyIn] FlatArray<int> queueIds)
    {
        OrganizationId = organizationId.OrEmpty();
        QueueIds = queueIds;
    }

    public string OrganizationId { get; }

    public FlatArray<int> QueueIds { get; }
}