namespace ItSupport.LspIspDip;

/// <summary>Построенный маршрут до АРМ.</summary>
public sealed record Route(
    double DistanceKm,
    int DurationMinutes,
    IReadOnlyList<string> Steps);
