namespace ItSupport.LspIspDip;

/// <summary>
/// Имитация удалённого сеанса. Реальный сеанс (RDP, SSH, агент) реализует
/// тот же <see cref="IRemoteSession"/> и подключается в Program.cs.
/// </summary>
public sealed class ConsoleRemoteSession : IRemoteSession
{
    public ConsoleRemoteSession(string workstationId)
    {
        WorkstationId = workstationId;
    }

    public string WorkstationId { get; }

    public string Execute(string command)
        => command switch
        {
            "clean-temp-files" =>
                "Очищены временные и архивные файлы, освобождено 32 ГБ.",
            "reset-network-adapter" =>
                "Сетевой адаптер перезапущен, получен IP-адрес, связь восстановлена.",
            _ => "Команда выполнена."
        };

    public void Dispose()
    {
        // В прототипе ресурсов нет; в реальной реализации здесь закрывается сеанс.
    }
}
