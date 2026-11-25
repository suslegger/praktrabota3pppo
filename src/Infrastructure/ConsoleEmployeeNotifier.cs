namespace ItSupport.LspIspDip;

/// <summary>Уведомление сотрудника через консоль.</summary>
public sealed class ConsoleEmployeeNotifier : IEmployeeNotifier
{
    public void Send(string employee, string message)
    {
        var previousColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.DarkGreen;

        try
        {
            Console.WriteLine($"[Уведомление] Сообщение сотруднику {employee} отправлено.");
        }
        finally
        {
            Console.ForegroundColor = previousColor;
        }
    }
}
