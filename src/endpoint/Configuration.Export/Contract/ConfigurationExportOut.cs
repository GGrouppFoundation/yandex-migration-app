using GarageGroup.Infra;

namespace GGroupp.Yandex.Migration;

public sealed record class ConfigurationExportOut
{
    [RootBodyOut("application/zip")]
    public required byte[] File { get; init; }
}