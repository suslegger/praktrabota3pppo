using ItSupport.Domain;

namespace ItSupport.Data;

/// <summary>Демонстрационный источник телеметрии.</summary>
public sealed class DemoTelemetrySource : ITelemetrySource
{
    private readonly IReadOnlyDictionary<string, Telemetry> _snapshots;

    public DemoTelemetrySource(IReadOnlyDictionary<string, Telemetry> snapshots)
    {
        _snapshots = snapshots;
    }

    public Telemetry Read(string workstationId)
        => _snapshots.TryGetValue(workstationId, out var telemetry)
            ? telemetry
            : throw new KeyNotFoundException($"Нет данных телеметрии для АРМ {workstationId}.");
}
