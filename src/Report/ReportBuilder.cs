using System.Text;

namespace ItSupport.BuilderReport;

/// <summary>
/// Абстрактный строитель. Определяет шаги создания отчёта,
/// но не знает, в каком формате будет готовый документ.
/// </summary>
public abstract class ReportBuilder
{
    protected readonly StringBuilder Content = new();

    public abstract void BuildHeader(IncidentData data);

    public abstract void BuildEquipment(IncidentData data);

    public abstract void BuildTelemetry(IncidentData data);

    public abstract void BuildDiagnostics(IncidentData data);

    public abstract void BuildRemediation(IncidentData data);

    public abstract void BuildLlmReply(IncidentData data);

    /// <summary>Возвращает готовый продукт.</summary>
    public abstract IncidentReport GetResult();

    protected void AppendLine(string text = "") => Content.AppendLine(text);
}
