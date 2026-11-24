using ItSupport.Domain;

namespace ItSupport.Remediation;

/// <summary>
/// Способ устранения неисправности. Новая стратегия добавляется отдельным классом без изменения существующих.
/// </summary>
public interface IRemediationStrategy
{
    /// <summary>Название стратегии для журнала и интерфейса оператора.</summary>
    string Name { get; }

    /// <summary>Может ли стратегия обработать данную неисправность.</summary>
    bool CanHandle(Fault fault);

    /// <summary>Выполняет устранение и возвращает результат.</summary>
    RemediationResult Remediate(RemediationContext context);
}
