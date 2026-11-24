using ItSupport.Domain;

namespace ItSupport.Remediation;

/// <summary>
/// Выбирает первую подходящую стратегию устранения.
/// Новые стратегии подключаются в точке сборки приложения (Program.cs)
/// и не требуют изменения этого класса.
/// </summary>
public sealed class RemediationPlanner
{
    private readonly IReadOnlyList<IRemediationStrategy> _strategies;

    public RemediationPlanner(IEnumerable<IRemediationStrategy> strategies)
    {
        _strategies = strategies.ToList();
    }

    public IRemediationStrategy Select(Fault fault)
        => _strategies.FirstOrDefault(strategy => strategy.CanHandle(fault))
           ?? throw new InvalidOperationException(
               $"Для неисправности {fault.Code} не зарегистрирована стратегия устранения.");
}
