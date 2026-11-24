using ItSupport.Domain;

namespace ItSupport.Remediation;

/// <summary>
/// Результат работы стратегии: удалось ли устранить неисправность,
/// что записать в заявку, нужен ли маршрут и журнал действий для оператора.
/// </summary>
public sealed record RemediationResult(
    bool IsResolved,
    string Summary,
    Route? Route,
    IReadOnlyList<string> Log);
