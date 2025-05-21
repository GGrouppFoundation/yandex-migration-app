using System;
using GarageGroup.Infra;

namespace GGroupp.Yandex.Migration;

public sealed record class QueueListGetIn
{
    public QueueListGetIn(
        [QueryIn] string organizationId)
    {
        OrganizationId = organizationId.OrEmpty();
    }

    public string OrganizationId { get; }
}