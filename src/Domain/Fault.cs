namespace ItSupport.Domain;

/// <summary>Обнаруженная неисправность АРМ.</summary>
/// <param name="Code">Машинный код неисправности (например, DISK_FULL).</param>
/// <param name="Title">Человекочитаемое название.</param>
/// <param name="Details">Пояснение для оператора.</param>
/// <param name="CanBeFixedRemotely">Можно ли устранить неисправность без выезда.</param>
public sealed record Fault(
    string Code,
    string Title,
    string Details,
    bool CanBeFixedRemotely);
