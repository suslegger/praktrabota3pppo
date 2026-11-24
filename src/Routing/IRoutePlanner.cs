using ItSupport.Domain;

namespace ItSupport.Routing;

/// <summary>
/// Построение маршрута от оператора до АРМ.
/// Другой способ (общественный транспорт, пешком) — новая реализация интерфейса.
/// </summary>
public interface IRoutePlanner
{
    Route Build(Location from, Location to);
}
