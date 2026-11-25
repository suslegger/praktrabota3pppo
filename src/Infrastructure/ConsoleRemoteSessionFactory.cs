namespace ItSupport.LspIspDip;

/// <summary>Создаёт консольные (имитационные) удалённые сеансы.</summary>
public sealed class ConsoleRemoteSessionFactory : IRemoteSessionFactory
{
    public IRemoteSession Open(Workstation workstation)
        => new ConsoleRemoteSession(workstation.Id);
}
