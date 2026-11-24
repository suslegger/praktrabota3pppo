namespace ItSupport.Presentation;

/// <summary>
/// Абстракция консольного интерфейса. Благодаря ей сценарий обработки заявки
/// не зависит от Console и может быть проверен с помощью тестового двойника.
/// </summary>
public interface IUserInterface
{
    void Clear();

    void Header(string text);

    void Info(string text);

    void Success(string text);

    void Warning(string text);

    void Error(string text);

    string Ask(string prompt);

    void Pause(string message = "Нажмите Enter, чтобы продолжить...");
}
