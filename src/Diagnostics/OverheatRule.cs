using ItSupport.Domain;

namespace ItSupport.Diagnostics;

/// <summary>Правило: процессор перегревается.</summary>
public sealed class OverheatRule : IFaultDiagnosticRule
{
    private const double MaximumTemperature = 85.0;

    public Fault? Inspect(Telemetry telemetry)
    {
        if (telemetry.CpuTemperature < MaximumTemperature)
        {
            return null;
        }

        return new Fault(
            Code: "OVERHEAT",
            Title: "Перегрев процессора",
            Details: $"Температура {telemetry.CpuTemperature:F0} °C (порог — {MaximumTemperature:F0} °C).",
            CanBeFixedRemotely: false);
    }
}
