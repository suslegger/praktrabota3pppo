using ItSupport.Domain;

namespace ItSupport.Diagnostics;

/// <summary>Правило: блок питания АРМ неисправен.</summary>
public sealed class PowerSupplyRule : IFaultDiagnosticRule
{
    public Fault? Inspect(Telemetry telemetry)
    {
        if (telemetry.PowerSupplyOk)
        {
            return null;
        }

        return new Fault(
            Code: "PSU_FAILURE",
            Title: "Неисправность блока питания",
            Details: "Питание нестабильно, требуется осмотр оборудования.",
            CanBeFixedRemotely: false);
    }
}
