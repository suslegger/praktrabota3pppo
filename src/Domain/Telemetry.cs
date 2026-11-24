namespace ItSupport.Domain;

/// <summary>Снимок телеметрии, считанный с АРМ.</summary>
public sealed record Telemetry(
    double DiskFreePercent,
    bool NetworkAvailable,
    double CpuTemperature,
    bool PowerSupplyOk);
