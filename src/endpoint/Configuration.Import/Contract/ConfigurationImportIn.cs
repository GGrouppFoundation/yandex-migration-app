using System;
using System.IO;
using GarageGroup.Infra;

namespace GGroupp.Yandex.Migration;

public sealed record class ConfigurationImportIn
{
    public ConfigurationImportIn(
        [RouteIn] string organizationId,
        [RootBodyIn("application/zip")] Stream file)
    {
        OrganizationId = organizationId.OrEmpty();
        File = file;
    }

    public string OrganizationId { get; }

    public Stream File { get; }
}