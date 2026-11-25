namespace ItSupport.LspIspDip;

/// <summary>
/// Базовый класс стратегии устранения неисправности.
/// Контракт для наследников:
/// <list type="bullet">
/// <item><see cref="CanHandle"/> не выбрасывает исключений и не меняет состояние;</item>
/// <item><see cref="Execute"/> для поддерживаемой неисправности возвращает результат,
/// а не null, и не сужает поведение базового класса.</item>
/// </list>
/// Планировщик работает только с этим базовым типом, поэтому любая
/// корректная реализация может быть подставлена без изменения его кода (LSP).
/// </summary>
public abstract class Remediation
{
    public abstract string Name { get; }

    public abstract bool CanHandle(Fault fault);

    public abstract RemediationResult Execute(RemediationContext context);
}
