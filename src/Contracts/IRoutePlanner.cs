namespace ItSupport.LspIspDip;

/// <summary>Построение маршрута от оператора до АРМ.</summary>
public interface IRoutePlanner
{
    Route Build(Location from, Location to);
}
