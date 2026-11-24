using System.Globalization;

namespace ItSupport.Beginner;

/// <summary>
/// Прототип платформы сопровождения ИТ-инфраструктуры.
/// </summary>
internal static class Program
{
    private static readonly string[] TicketNumbers = { "TCK-1001", "TCK-1002", "TCK-1003", "TCK-1004" };
    private static readonly string[] TicketStatuses = { "NEW", "NEW", "NEW", "NEW" };
    private static readonly string[] TicketWorkstations = { "WS-101", "WS-205", "WS-310", "WS-415" };
    private static readonly string[] TicketEmployees = { "Петрова А.С.", "Сидоров П.Н.", "Кузнецов Д.В.", "Смирнова Е.А." };
    private static readonly string[] TicketDescriptions =
    {
        "Не могу сохранить отчёт: «недостаточно места на диске»",
        "Пропал интернет, не открываются сайты",
        "Компьютер сам выключается во время работы",
        "Монитор не включается"
    };

    private static readonly string[][] Workstations =
    {
        new[] { "WS-101", "АРМ бухгалтера", "каб. 214", "Кемерово, пр. Октябрьский, 34", "2", "да", "62", "ок" },
        new[] { "WS-205", "АРМ менеджера", "каб. 108", "Кемерово, пр. Октябрьский, 34", "55", "нет", "58", "ок" },
        new[] { "WS-310", "АРМ кладовщика", "корпус 2, каб. 3", "Кемерово, ул. Терешковой, 41", "40", "да", "96", "ок" },
        new[] { "WS-415", "АРМ диспетчера", "каб. 320", "Кемерово, ул. Весенняя, 12", "30", "да", "45", "сбой" }
    };

    private static readonly double[] WorkstationLat = { 55.3547, 55.3548, 55.3400, 55.3552 };
    private static readonly double[] WorkstationLon = { 86.0873, 86.0875, 86.1200, 86.0862 };

    private static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        const string operatorName = "Иванов И.И.";
        const string operatorAddress = "Кемерово, ул. Весенняя, 12";
        const double operatorLat = 55.3550;
        const double operatorLon = 86.0860;

        while (true)
        {
            if (!Console.IsOutputRedirected)
            {
                Console.Clear();
            }

            Console.WriteLine("==========================================================");
            Console.WriteLine(" Интеллектуальная платформа сопровождения ИТ-инфраструктуры");
            Console.WriteLine("==========================================================");
            Console.WriteLine("Оператор: " + operatorName + " (" + operatorAddress + ")");
            Console.WriteLine();
            Console.WriteLine("Заявки:");

            for (var i = 0; i < TicketNumbers.Length; i++)
            {
                Console.WriteLine((i + 1) + ". [" + TicketStatuses[i] + "] " + TicketNumbers[i] +
                                  " — " + TicketDescriptions[i]);
            }

            Console.WriteLine("0. Выход");
            Console.Write("Выберите заявку: ");
            var input = Console.ReadLine();
            if (input == "0")
            {
                break;
            }

            if (!int.TryParse(input, out var number) || number < 1 || number > TicketNumbers.Length)
            {
                Console.WriteLine("Такой заявки нет. Нажмите Enter...");
                Console.ReadLine();
                continue;
            }

            ProcessTicket(number - 1, operatorAddress, operatorLat, operatorLon);
            Console.WriteLine();
            Console.WriteLine("Нажмите Enter, чтобы вернуться к списку заявок...");
            Console.ReadLine();
        }
    }

    private static void ProcessTicket(int index, string operatorAddress, double operatorLat, double operatorLon)
    {
        if (TicketStatuses[index] != "NEW")
        {
            Console.WriteLine("Заявка уже обработана.");
            return;
        }

        TicketStatuses[index] = "IN_PROGRESS";

        Console.WriteLine();
        Console.WriteLine("[Оператор] Заявка " + TicketNumbers[index] + " принята в работу.");
        Console.WriteLine("[Сотрудник] " + TicketEmployees[index] + ": " + TicketDescriptions[index]);

        var wsIndex = -1;
        for (var i = 0; i < Workstations.Length; i++)
        {
            if (Workstations[i][0] == TicketWorkstations[index])
            {
                wsIndex = i;
            }
        }

        var ws = Workstations[wsIndex];
        Console.WriteLine("[Инвентаризация] " + ws[0] + " — " + ws[1] + ", " + ws[2] + ".");

        var diskFree = double.Parse(ws[4], CultureInfo.InvariantCulture);
        var hasNetwork = ws[5] == "да";
        var temperature = double.Parse(ws[6], CultureInfo.InvariantCulture);
        var psuOk = ws[7] == "ок";

        Console.WriteLine("[Мониторинг] Диск: " + diskFree + "% свободно; сеть: " +
                          (hasNetwork ? "доступна" : "нет") + "; температура: " + temperature +
                          " °C; питание: " + (psuOk ? "норма" : "сбой") + ".");

        var fault = "";
        var faultTitle = "";
        var canFixRemotely = false;

        if (diskFree < 5)
        {
            fault = "DISK_FULL";
            faultTitle = "Недостаточно места на диске";
            canFixRemotely = true;
        }
        else if (!hasNetwork)
        {
            fault = "NO_NETWORK";
            faultTitle = "Отсутствует сеть";
            canFixRemotely = true;
        }
        else if (temperature >= 85)
        {
            fault = "OVERHEAT";
            faultTitle = "Перегрев процессора";
        }
        else if (!psuOk)
        {
            fault = "PSU_FAILURE";
            faultTitle = "Неисправность блока питания";
        }

        if (fault == "")
        {
            Console.WriteLine("[Диагностика] Неисправностей не обнаружено.");
            TicketStatuses[index] = "RESOLVED";
            return;
        }

        Console.WriteLine("[Диагностика] " + fault + ": " + faultTitle);

        if (canFixRemotely)
        {
            Console.WriteLine("[Удалённое подключение] Открываю сеанс с " + ws[0] + "...");
            Console.WriteLine("[Удалённое подключение] Сеанс установлен (" + ws[1] + ", " + ws[2] + ").");

            if (fault == "DISK_FULL")
            {
                Console.WriteLine("[Удалённое устранение] Очищены временные файлы, освобождено 32 ГБ.");
            }
            else if (fault == "NO_NETWORK")
            {
                Console.WriteLine("[Удалённое устранение] Сетевой адаптер перезапущен, связь восстановлена.");
            }

            Console.WriteLine("[Удалённое подключение] Сеанс завершён.");
            TicketStatuses[index] = "RESOLVED";

            Console.WriteLine();
            Console.WriteLine("[LLM] Здравствуйте, " + TicketEmployees[index] + "! Неисправность «" +
                              faultTitle + "» на АРМ " + ws[0] + " устранена удалённо. " +
                              "Пожалуйста, проверьте работу и подтвердите закрытие заявки " +
                              TicketNumbers[index] + ".");
        }
        else
        {
            Console.WriteLine("[Локация] АРМ " + ws[0] + ": " + ws[3] + ", " + ws[2]);
            Console.WriteLine("[Локация] Оператор: " + operatorAddress);

            if (ws[3] == operatorAddress)
            {
                Console.WriteLine("[Маршрут] АРМ в том же здании — маршрут не требуется.");
            }
            else
            {
                var dLat = (WorkstationLat[wsIndex] - operatorLat) * Math.PI / 180;
                var dLon = (WorkstationLon[wsIndex] - operatorLon) * Math.PI / 180;
                var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                        Math.Cos(operatorLat * Math.PI / 180) *
                        Math.Cos(WorkstationLat[wsIndex] * Math.PI / 180) *
                        Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
                var distance = Math.Round(6371 * 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a)) * 1.35, 1);
                var minutes = (int)Math.Ceiling(distance / 35 * 60) + 5;

                Console.WriteLine("[Маршрут] " + distance + " км, ориентировочно " + minutes + " мин.");
                Console.WriteLine("  1. Выйдите из здания: " + operatorAddress + ".");
                Console.WriteLine("  2. Постройте маршрут в навигаторе до адреса: " + ws[3] + ".");
                Console.WriteLine("  3. По прибытии отметьте выезд в системе.");
            }

            TicketStatuses[index] = "ESCALATED";

            var arrival = ws[3] == operatorAddress
                ? "Инженер находится в том же здании и скоро подойдёт."
                : "Инженер уже в пути.";
            Console.WriteLine();
            Console.WriteLine("[LLM] Здравствуйте, " + TicketEmployees[index] + "! По заявке " +
                              TicketNumbers[index] + " зарегистрирована неисправность «" + faultTitle + "». " +
                              arrival + " Пожалуйста, не выключайте АРМ и ожидайте инженера.");
        }

        Console.WriteLine("[Статус] " + TicketNumbers[index] + ": " + TicketStatuses[index]);
    }
}
