using ItSupport.Domain;

namespace ItSupport.Diagnostics;

/// <summary>
/// Прогоняет телеметрию через набор правил.
/// </summary>
public sealed class FaultDiagnosticsService
{
    private readonly IReadOnlyList<IFaultDiagnosticRule> _rules;

    public FaultDiagnosticsService(IEnumerable<IFaultDiagnosticRule> rules)
    {
        _rules = rules.ToList();
    }

    public IReadOnlyList<Fault> Diagnose(Telemetry telemetry)
        => _rules
            .Select(rule => rule.Inspect(telemetry))
            .OfType<Fault>()
            .ToList();
}
