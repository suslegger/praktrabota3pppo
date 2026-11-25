namespace ItSupport.LspIspDip;

/// <summary>Создаёт удалённые сеансы. Клиенту не нужно знать реализацию сеанса.</summary>
public interface IRemoteSessionFactory
{
    IRemoteSession Open(Workstation workstation);
}
