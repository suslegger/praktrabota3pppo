namespace ItSupport.LspIspDip;

/// <summary>Источник телеметрии АРМ.</summary>
public interface ITelemetrySource
{
    /// <exception cref="KeyNotFoundException">Для АРМ нет данных телеметрии.</exception>
    Telemetry Read(string workstationId);
}
