namespace ItSupport.Domain;

/// <summary>
/// Адрес и географические координаты объекта (АРМ или оператора).
/// </summary>
public sealed record Location(string Address, double Latitude, double Longitude)
{
    private const double EarthRadiusKm = 6371.0;

    /// <summary>Расстояние по прямой (формула гаверсинуса), км.</summary>
    public double DistanceKmTo(Location other)
    {
        var dLat = ToRadians(other.Latitude - Latitude);
        var dLon = ToRadians(other.Longitude - Longitude);

        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadians(Latitude)) * Math.Cos(ToRadians(other.Latitude)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        return EarthRadiusKm * 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
    }

    private static double ToRadians(double degrees) => degrees * Math.PI / 180.0;
}
