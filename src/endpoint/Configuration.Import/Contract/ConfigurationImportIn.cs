using System;
using System.IO;
using GarageGroup.Infra;

namespace GGroupp.Yandex.Migration;

public sealed record class ConfigurationImportIn
{
    public ConfigurationImportIn(
        [RouteIn] string organizationId,
        [HeaderIn] int contentLength, // how to pass without showing in swagger?
        [RootBodyIn("application/zip")] Stream file)
    {
        OrganizationId = organizationId.OrEmpty();
        ContentLength = contentLength;
        File = file;
    }

    public string OrganizationId { get; }

    public int ContentLength { get; }

    public Stream File { get; }
}