namespace ItSupport.BuilderReport;

/// <summary>
/// Распорядитель (Director). Знает порядок шагов построения отчёта,
/// но не знает, какой конкретный строитель используется.
/// </summary>
public sealed class ReportDirector
{
    private readonly ReportBuilder _builder;

    public ReportDirector(ReportBuilder builder)
    {
        _builder = builder;
    }

    public IncidentReport Construct(IncidentData data)
    {
        _builder.BuildHeader(data);
        _builder.BuildEquipment(data);
        _builder.BuildTelemetry(data);
        _builder.BuildDiagnostics(data);
        _builder.BuildRemediation(data);
        _builder.BuildLlmReply(data);

        return _builder.GetResult();
    }
}
