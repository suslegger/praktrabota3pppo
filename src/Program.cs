using System.Text;

namespace ItSupport.LspIspDip;

/// <summary>
/// Точка сборки приложения. Здесь высокоуровневые классы получают конкретные
/// реализации абстракций (DIP): сценарий обработки заявки не создаёт их сам.
/// </summary>
internal static class Program
{
    private static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        var ui = new ConsoleUserInterface();

        var operatorUser = new Operator(
            "Иванов И.И.",
            new Location("Кемерово, ул. Весенняя, 12", 55.3550, 86.0860));

        // LSP: массив базового типа Workstation хранит разные производные типы.
        var catalog = new InMemoryWorkstationCatalog(new Workstation[]
        {
            new AccountantWorkstation(
                "WS-101", "АРМ бухгалтера", "каб. 214", "Петрова А.С.",
                new Location("Кемерово, пр. Октябрьский, 34", 55.3547, 86.0873)),
            new ManagerWorkstation(
                "WS-205", "АРМ менеджера", "каб. 108", "Сидоров П.Н.",
                new Location("Кемерово, пр. Октябрьский, 34", 55.3548, 86.0875)),
            new WarehouseWorkstation(
                "WS-310", "АРМ кладовщика", "корпус 2, каб. 3", "Кузнецов Д.В.",
                new Location("Кемерово, ул. Терешковой, 41", 55.3400, 86.1200)),
            new DispatcherWorkstation(
                "WS-415", "АРМ диспетчера", "каб. 320", "Смирнова Е.А.",
                new Location("Кемерово, ул. Весенняя, 12", 55.3552, 86.0862))
        });

        var telemetry = new DemoTelemetrySource(new Dictionary<string, Telemetry>
        {
            ["WS-101"] = new(DiskFreePercent: 2, NetworkAvailable: true, CpuTemperature: 62, PowerSupplyOk: true),
            ["WS-205"] = new(DiskFreePercent: 55, NetworkAvailable: false, CpuTemperature: 58, PowerSupplyOk: true),
            ["WS-310"] = new(DiskFreePercent: 40, NetworkAvailable: true, CpuTemperature: 96, PowerSupplyOk: true),
            ["WS-415"] = new(DiskFreePercent: 30, NetworkAvailable: true, CpuTemperature: 45, PowerSupplyOk: false)
        });

        var tickets = new InMemoryTicketRepository(new[]
        {
            new Ticket("TCK-1001", "WS-101", "Петрова А.С.",
                "Не могу сохранить отчёт: «недостаточно места на диске»"),
            new Ticket("TCK-1002", "WS-205", "Сидоров П.Н.",
                "Пропал интернет, не открываются сайты"),
            new Ticket("TCK-1003", "WS-310", "Кузнецов Д.В.",
                "Компьютер сам выключается во время работы"),
            new Ticket("TCK-1004", "WS-415", "Смирнова Е.А.",
                "Монитор не включается")
        });

        var diagnostics = new RuleBasedDiagnostics(new IFaultRule[]
        {
            new DiskSpaceRule(),
            new NetworkRule(),
            new OverheatRule(),
            new PowerSupplyRule()
        });

        var sessions = new ConsoleRemoteSessionFactory();
        var planner = new FirstMatchRemediationPlanner(new Remediation[]
        {
            new RemoteDiskCleanup(sessions),
            new RemoteNetworkReset(sessions),
            new OnsiteVisitRemediation(new CarRoutePlanner())
        });

        var incidents = new IncidentService(
            catalog,
            telemetry,
            diagnostics,
            planner,
            tickets,
            new OfflineLlmAssistant(),
            new ConsoleEmployeeNotifier(),
            ui,
            operatorUser);

        new SupportConsoleApp(tickets, incidents, ui, operatorUser).Run();
    }
}
