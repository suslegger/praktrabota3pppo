using ItSupport.Domain;

namespace ItSupport.Data;

/// <summary>
/// Источник телеметрии АРМ. В реальной системе — агент мониторинга (здесь - подготовленные данные)
/// </summary>
public interface ITelemetrySource
{
    /// <exception cref="KeyNotFoundException">Для АРМ нет данных телеметрии.</exception>
    Telemetry Read(string workstationId);
}
