namespace ItSupport.LspIspDip;

/// <summary>Прогоняет телеметрию через набор правил диагностики.</summary>
public sealed class RuleBasedDiagnostics : IDiagnosticsService
{
    private readonly IReadOnlyList<IFaultRule> _rules;

    public RuleBasedDiagnostics(IEnumerable<IFaultRule> rules)
    {
        _rules = rules.ToList();
    }

    public IReadOnlyList<Fault> Diagnose(Telemetry telemetry)
        => _rules
            .Select(rule => rule.Inspect(telemetry))
            .OfType<Fault>()
            .ToList();
}
