namespace ItSupport.LspIspDip;

/// <summary>
/// Один удалённый сеанс. Интерфейс намеренно узкий: клиенту нужны только
/// идентификатор АРМ, выполнение команды и освобождение ресурсов.
/// </summary>
public interface IRemoteSession : IDisposable
{
    string WorkstationId { get; }

    /// <summary>Выполняет команду и возвращает описание результата.</summary>
    string Execute(string command);
}
