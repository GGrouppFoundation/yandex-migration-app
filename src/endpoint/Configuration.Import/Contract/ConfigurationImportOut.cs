using System;
using GarageGroup.Infra;

namespace GGroupp.Yandex.Migration;

public sealed record class ConfigurationImportOut
{
    [RootBodyOut]
    public FlatArray<ImportResult> ImportResults { get; init; }
}