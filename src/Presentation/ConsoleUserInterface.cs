using System.Text;

namespace ItSupport.Presentation;

/// <summary>Реализация интерфейса поверх системной консоли.</summary>
public sealed class ConsoleUserInterface : IUserInterface
{
    public void Clear()
    {
        try
        {
            Console.Clear();
        }
        catch (IOException)
        {
            // Вывод перенаправлен (например, в файл) — очистка экрана недоступна.
        }
    }

    public void Header(string text)
    {
        WriteLine(string.Empty, ConsoleColor.Gray);
        WriteLine(text, ConsoleColor.Cyan);
        WriteLine(new string('-', Math.Min(text.Length, 64)), ConsoleColor.DarkCyan);
    }

    public void Info(string text) => WriteLine(text, ConsoleColor.Gray);

    public void Success(string text) => WriteLine(text, ConsoleColor.Green);

    public void Warning(string text) => WriteLine(text, ConsoleColor.Yellow);

    public void Error(string text) => WriteLine(text, ConsoleColor.Red);

    public string Ask(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine() ?? string.Empty;
    }

    public void Pause(string message = "Нажмите Enter, чтобы продолжить...")
    {
        Console.WriteLine();
        Console.Write(message);
        Console.ReadLine();
    }

    private static void WriteLine(string text, ConsoleColor color)
    {
        var previousColor = Console.ForegroundColor;
        Console.ForegroundColor = color;

        try
        {
            Console.WriteLine(text);
        }
        finally
        {
            // Цвет возвращается даже при ошибке вывода.
            Console.ForegroundColor = previousColor;
        }
    }
}
