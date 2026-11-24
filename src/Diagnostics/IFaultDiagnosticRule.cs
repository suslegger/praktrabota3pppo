using ItSupport.Domain;

namespace ItSupport.Diagnostics;

/// <summary>
/// Одно правило диагностики. Новая проверка добавляется отдельным классом,
/// существующие правила при этом не изменяются (OCP).
/// </summary>
public interface IFaultDiagnosticRule
{
    /// <summary>Возвращает неисправность или <c>null</c>, если правило не сработало.</summary>
    Fault? Inspect(Telemetry telemetry);
}
