namespace ItSupport.LspIspDip;

/// <summary>Выбор способа устранения неисправности.</summary>
public interface IRemediationPlanner
{
    /// <exception cref="InvalidOperationException">Подходящая стратегия не найдена.</exception>
    Remediation Select(Fault fault);
}
