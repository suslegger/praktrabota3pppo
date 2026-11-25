namespace ItSupport.LspIspDip;

/// <summary>Диагностика телеметрии.</summary>
public interface IDiagnosticsService
{
    IReadOnlyList<Fault> Diagnose(Telemetry telemetry);
}
