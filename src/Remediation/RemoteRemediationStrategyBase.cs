using ItSupport.Domain;

namespace ItSupport.Remediation;

/// <summary>
/// Общая часть стратегий удалённого устранения: открытие сеанса,
/// журналирование и закрытие сеанса. Конкретное действие задаёт наследник.
/// </summary>
public abstract class RemoteRemediationStrategyBase : IRemediationStrategy
{
    public abstract string Name { get; }

    public abstract bool CanHandle(Fault fault);

    /// <summary>Выполняет конкретное исправление и возвращает описание для журнала.</summary>
    protected abstract string PerformFix(RemediationContext context);

    public RemediationResult Remediate(RemediationContext context)
    {
        var log = new List<string>
        {
            $"[Удалённое подключение] Открываю сеанс с {context.Workstation.Id}...",
            $"[Удалённое подключение] Сеанс установлен ({context.Workstation.Name}, {context.Workstation.Room}).",
            $"[{Name}] {PerformFix(context)}",
            "[Удалённое подключение] Сеанс завершён."
        };

        return new RemediationResult(
            IsResolved: true,
            Summary: $"Неисправность «{context.Fault.Title}» устранена удалённо.",
            Route: null,
            Log: log);
    }
}
