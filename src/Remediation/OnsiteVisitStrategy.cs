using ItSupport.Domain;
using ItSupport.Routing;

namespace ItSupport.Remediation;

/// <summary>
/// Устранение неисправности с выездом инженера.
/// Сравнивает местоположение АРМ и оператора и при необходимости строит маршрут.
/// </summary>
public sealed class OnsiteVisitStrategy : IRemediationStrategy
{
    private readonly IRoutePlanner _routePlanner;
    private readonly double _sameBuildingThresholdKm;

    public OnsiteVisitStrategy(IRoutePlanner routePlanner, double sameBuildingThresholdKm = 0.3)
    {
        _routePlanner = routePlanner;
        _sameBuildingThresholdKm = sameBuildingThresholdKm;
    }

    /// <summary>Стратегия-запасной вариант: обрабатывает всё, что не удалось устранить удалённо.</summary>
    public string Name => "Выезд инженера";

    public bool CanHandle(Fault fault) => true;

    public RemediationResult Remediate(RemediationContext context)
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
