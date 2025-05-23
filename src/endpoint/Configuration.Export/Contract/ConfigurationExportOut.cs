using GarageGroup.Infra;

namespace GGroupp.Yandex.Migration;

public sealed record class ConfigurationExportOut
{
    [JsonBodyOut("application/zip")]
    public required byte[] File { get; init; }
}