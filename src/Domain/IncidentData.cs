namespace ItSupport.BuilderReport;

/// <summary>Снимок телеметрии АРМ.</summary>
public sealed record TelemetrySnapshot(
    double DiskFreePercent,
    bool NetworkAvailable,
    double CpuTemperature,
    bool PowerSupplyOk);

/// <summary>Обнаруженная неисправность.</summary>
public sealed record FaultInfo(
    string Code,
    string Title,
    string Details,
    bool CanBeFixedRemotely);

/// <summary>Результат устранения неисправности.</summary>
public sealed record RemediationInfo(
    string Strategy,
    string Summary,
    double? RouteDistanceKm,
    int? RouteMinutes);

/// <summary>
/// Исходные данные для построения отчёта об инциденте.
/// Продукт строится не из базы данных, а из этого снимка.
/// </summary>
public sealed record IncidentData(
    string TicketNumber,
    string Employee,
    string WorkstationId,
    string WorkstationName,
    string Room,
    string Address,
    TelemetrySnapshot Telemetry,
    FaultInfo Fault,
    RemediationInfo Remediation,
    string LlmReply);
