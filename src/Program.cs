using System.Text;
using ItSupport.Ai;
using ItSupport.Application;
using ItSupport.Data;
using ItSupport.Diagnostics;
using ItSupport.Domain;
using ItSupport.Presentation;
using ItSupport.Remediation;
using ItSupport.Routing;

namespace ItSupport;

/// <summary>
/// Точка сборки приложения
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

        var workstations = new InMemoryWorkstationRepository(new[]
        {
            new Workstation("WS-101", "АРМ бухгалтера", "каб. 214", "Петрова А.С.",
                new Location("Кемерово, пр. Октябрьский, 34", 55.3547, 86.0873)),
            new Workstation("WS-205", "АРМ менеджера", "каб. 108", "Сидоров П.Н.",
                new Location("Кемерово, пр. Октябрьский, 34", 55.3548, 86.0875)),
            new Workstation("WS-310", "АРМ кладовщика", "корпус 2, каб. 3", "Кузнецов Д.В.",
                new Location("Кемерово, ул. Терешковой, 41", 55.3400, 86.1200)),
            new Workstation("WS-415", "АРМ диспетчера", "каб. 320", "Смирнова Е.А.",
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

        var diagnostics = new FaultDiagnosticsService(new IFaultDiagnosticRule[]
        {
            new DiskSpaceRule(),
            new NetworkRule(),
            new OverheatRule(),
            new PowerSupplyRule()
        });

        var remediation = new RemediationPlanner(new IRemediationStrategy[]
        {
            new RemoteDiskCleanupStrategy(),
            new RemoteNetworkResetStrategy(),
            new OnsiteVisitStrategy(new CarRoutePlanner(), sameBuildingThresholdKm: 0.3)
        });

        var incidents = new IncidentService(
            workstations,
            telemetry,
            diagnostics,
            remediation,
            new OfflineLlmAssistant(),
            tickets,
            ui,
            operatorUser);

        new SupportConsoleApp(tickets, incidents, ui, operatorUser).Run();
    }
}
