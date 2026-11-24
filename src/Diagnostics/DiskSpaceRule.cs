using ItSupport.Domain;

namespace ItSupport.Diagnostics;

/// <summary>Правило: на системном диске почти не осталось свободного места.</summary>
public sealed class DiskSpaceRule : IFaultDiagnosticRule
{
    private const double MinimumFreePercent = 5.0;

    public Fault? Inspect(Telemetry telemetry)
    {
        if (telemetry.DiskFreePercent >= MinimumFreePercent)
        {
            return null;
        }

        return new Fault(
            Code: "DISK_FULL",
            Title: "Недостаточно места на диске",
            Details: $"Свободно {telemetry.DiskFreePercent:F1}% (порог — {MinimumFreePercent:F1}%).",
            CanBeFixedRemotely: true);
    }
}
