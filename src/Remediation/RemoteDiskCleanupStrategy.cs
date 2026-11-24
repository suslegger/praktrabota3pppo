using ItSupport.Domain;

namespace ItSupport.Remediation;

/// <summary>Удалённая очистка диска АРМ.</summary>
public sealed class RemoteDiskCleanupStrategy : RemoteRemediationStrategyBase
{
    public override string Name => "Удалённое устранение";

    public override bool CanHandle(Fault fault) => fault.Code == "DISK_FULL";

    protected override string PerformFix(RemediationContext context)
        => "Очищены временные и архивные файлы, освобождено 32 ГБ.";
}
