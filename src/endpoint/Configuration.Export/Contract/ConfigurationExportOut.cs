using System.IO;
using GarageGroup.Infra;

namespace GGroupp.Yandex.Migration;

public sealed record class ConfigurationExportOut
{
    public ConfigurationExportOut(string fileName, Stream file)
    {
        ContentDisposition = $"attachment; filename=\"{fileName}\"";
        File = file;
    }

    [HeaderOut("Content-Disposition")]
    public string ContentDisposition { get; }

    [RootBodyOut("application/zip")]
    public Stream File { get; }
}