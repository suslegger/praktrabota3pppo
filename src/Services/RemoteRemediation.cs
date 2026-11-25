namespace ItSupport.LspIspDip;

/// <summary>
/// Общая часть стратегий удалённого устранения. Наследники задают только
/// признак поддерживаемой неисправности и команду для удалённого сеанса.
/// </summary>
public abstract class RemoteRemediation : Remediation
{
    private readonly IRemoteSessionFactory _sessions;

    protected RemoteRemediation(IRemoteSessionFactory sessions)
    {
        _sessions = sessions;
    }

    public override string Name => "Удалённое устранение";

    public override bool CanHandle(Fault fault)
        => fault.CanBeFixedRemotely && Handles(fault.Code);

    protected abstract bool Handles(string faultCode);

    protected abstract string Command { get; }

    public override RemediationResult Execute(RemediationContext context)
    {
        var log = new List<string>
        {
            $"[Удалённое подключение] Открываю сеанс с {context.Workstation.Id}..."
        };

        using var session = _sessions.Open(context.Workstation);
        log.Add($"[Удалённое подключение] Сеанс установлен " +
                $"({context.Workstation.Name}, {context.Workstation.Room}).");

        var fixResult = session.Execute(Command);
        log.Add($"[Удалённое устранение] {fixResult}");
        log.Add("[Удалённое подключение] Сеанс завершён.");

        return new RemediationResult(
            IsResolved: true,
            Summary: $"Неисправность «{context.Fault.Title}» устранена удалённо.",
            Route: null,
            Log: log);
    }
}

public sealed class RemoteDiskCleanup : RemoteRemediation
{
    public RemoteDiskCleanup(IRemoteSessionFactory sessions)
        : base(sessions)
    {
    }

    protected override bool Handles(string faultCode) => faultCode == "DISK_FULL";

    protected override string Command => "clean-temp-files";
}

public sealed class RemoteNetworkReset : RemoteRemediation
{
    public RemoteNetworkReset(IRemoteSessionFactory sessions)
        : base(sessions)
    {
    }

    protected override bool Handles(string faultCode) => faultCode == "NO_NETWORK";

    protected override string Command => "reset-network-adapter";
}
