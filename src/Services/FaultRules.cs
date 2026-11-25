namespace ItSupport.LspIspDip;

/// <summary>Одно правило диагностики телеметрии.</summary>
public interface IFaultRule
{
    Fault? Inspect(Telemetry telemetry);
}

public sealed class DiskSpaceRule : IFaultRule
{
    private const double MinimumFreePercent = 5.0;

    public Fault? Inspect(Telemetry telemetry)
        => telemetry.DiskFreePercent < MinimumFreePercent
            ? new DiskFullFault(telemetry.DiskFreePercent)
            : null;
}

public sealed class NetworkRule : IFaultRule
{
    public Fault? Inspect(Telemetry telemetry)
        => telemetry.NetworkAvailable ? null : new NetworkFault();
}

public sealed class OverheatRule : IFaultRule
{
    private const double MaximumTemperature = 85.0;

    public Fault? Inspect(Telemetry telemetry)
        => telemetry.CpuTemperature >= MaximumTemperature
            ? new OverheatFault(telemetry.CpuTemperature)
            : null;
}

public sealed class PowerSupplyRule : IFaultRule
{
    public Fault? Inspect(Telemetry telemetry)
        => telemetry.PowerSupplyOk ? null : new PowerSupplyFault();
}
