using ItSupport.Domain;

namespace ItSupport.Routing;

/// <summary>
/// Упрощённый автомобильный маршрут: расстояние по прямой умножается
/// на коэффициент кривизны дорог, время считается по средней скорости.
/// В реальной системе заменяется интеграцией с картографическим сервисом.
/// </summary>
public sealed class CarRoutePlanner : IRoutePlanner
{
    private const double RoadFactor = 1.35;
    private const double AverageSpeedKmH = 35.0;

    public Route Build(Location from, Location to)
    {
        var distanceKm = Math.Round(from.DistanceKmTo(to) * RoadFactor, 1);
        var durationMinutes = (int)Math.Ceiling(distanceKm / AverageSpeedKmH * 60) + 5;

        var steps = new List<string>
        {
            $"Выйдите из здания: {from.Address}.",
            $"Постройте маршрут в навигаторе до адреса: {to.Address}.",
            $"Ориентировочное расстояние — {distanceKm:F1} км, время в пути — {durationMinutes} мин.",
            "По прибытии отметьте выезд в системе."
        };

        return new Route(distanceKm, durationMinutes, steps);
    }
}
