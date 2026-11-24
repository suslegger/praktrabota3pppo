using ItSupport.Domain;

namespace ItSupport.Remediation;

/// <summary>Удалённый перезапуск сетевого адаптера.</summary>
public sealed class RemoteNetworkResetStrategy : RemoteRemediationStrategyBase
{
    public override string Name => "Удалённое устранение";

    public override bool CanHandle(Fault fault) => fault.Code == "NO_NETWORK";

    protected override string PerformFix(RemediationContext context)
        => "Сетевой адаптер перезапущен, получен IP-адрес, связь восстановлена.";
}
