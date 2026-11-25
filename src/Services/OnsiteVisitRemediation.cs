namespace ItSupport.LspIspDip;

/// <summary>
/// Выезд инженера. Зависит только от <see cref="IRoutePlanner"/>:
/// телеметрия, удалённые сеансы и уведомления этому классу не нужны (ISP).
/// </summary>
public sealed class OnsiteVisitRemediation : Remediation
{
    private readonly IRoutePlanner _routePlanner;
    private readonly double _sameBuildingThresholdKm;

    public OnsiteVisitRemediation(IRoutePlanner routePlanner, double sameBuildingThresholdKm = 0.3)
    {
        _routePlanner = routePlanner;
        _sameBuildingThresholdKm = sameBuildingThresholdKm;
    }

    public override string Name => "Выезд инженера";

    /// <summary>Стратегия-запасной вариант: подходит для любой неисправности.</summary>
    public override bool CanHandle(Fault fault) => true;

    public override RemediationResult Execute(RemediationContext context)
    {
        var workstation = context.Workstation;
        var log = new List<string>
        {
            $"[Локация] АРМ {workstation.Id}: {workstation.Location.Address}, {workstation.Room}",
            $"[Локация] Оператор: {context.Operator.Location.Address}"
        };

        var straightDistanceKm = workstation.Location.DistanceKmTo(context.Operator.Location);
        if (straightDistanceKm <= _sameBuildingThresholdKm)
        {
            log.Add("[Маршрут] АРМ находится в том же здании — маршрут не требуется.");
            return new RemediationResult(
                IsResolved: false,
                Summary: $"Требуется выезд в {workstation.Room}.",
                Route: null,
                Log: log);
        }

        var route = _routePlanner.Build(context.Operator.Location, workstation.Location);
        log.Add($"[Маршрут] {route.DistanceKm:F1} км, ориентировочно {route.DurationMinutes} мин.");
        log.AddRange(route.Steps.Select(step => $"  • {step}"));

        return new RemediationResult(
            IsResolved: false,
            Summary: $"Требуется выезд по адресу {workstation.Location.Address}.",
            Route: route,
            Log: log);
    }
}
