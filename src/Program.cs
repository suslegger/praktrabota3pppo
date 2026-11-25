using System.Text;

namespace ItSupport.BuilderReport;

/// <summary>
/// Клиент паттерна Строитель. Создаёт распорядителя с конкретным строителем,
/// получает готовый отчёт и сохраняет его в файл.
/// </summary>
internal static class Program
{
    private static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        var incident = CreateDemoIncident();

        var textDirector = new ReportDirector(new TextReportBuilder());
        var textReport = textDirector.Construct(incident);

        var xmlDirector = new ReportDirector(new XmlReportBuilder());
        var xmlReport = xmlDirector.Construct(incident);

        Console.WriteLine("=== Текстовый отчёт ===");
        textReport.Print();

        Console.WriteLine();
        Console.WriteLine("=== XML-отчёт ===");
        xmlReport.Print();

        var outputDirectory = Path.Combine(Directory.GetCurrentDirectory(), "output");
        Directory.CreateDirectory(outputDirectory);

        var textPath = Path.Combine(outputDirectory, "incident-report.txt");
        var xmlPath = Path.Combine(outputDirectory, "incident-report.xml");

        File.WriteAllText(textPath, textReport.Content, Encoding.UTF8);
        File.WriteAllText(xmlPath, xmlReport.Content, Encoding.UTF8);

        Console.WriteLine();
        Console.WriteLine($"Файлы сохранены: {textPath}, {xmlPath}");
    }

    private static IncidentData CreateDemoIncident()
        => new(
            TicketNumber: "TCK-1003",
            Employee: "Кузнецов Д.В.",
            WorkstationId: "WS-310",
            WorkstationName: "АРМ кладовщика",
            Room: "корпус 2, каб. 3",
            Address: "Кемерово, ул. Терешковой, 41",
            Telemetry: new TelemetrySnapshot(
                DiskFreePercent: 40,
                NetworkAvailable: true,
                CpuTemperature: 96,
                PowerSupplyOk: true),
            Fault: new FaultInfo(
                Code: "OVERHEAT",
                Title: "Перегрев процессора",
                Details: "Температура 96 °C (порог — 85 °C).",
                CanBeFixedRemotely: false),
            Remediation: new RemediationInfo(
                Strategy: "Выезд инженера",
                Summary: "Требуется выезд по адресу Кемерово, ул. Терешковой, 41.",
                RouteDistanceKm: 3.7,
                RouteMinutes: 12),
            LlmReply: "Здравствуйте, Кузнецов Д.В.! По заявке TCK-1003 " +
                      "зарегистрирована неисправность «Перегрев процессора». " +
                      "Требуется выезд по адресу Кемерово, ул. Терешковой, 41. " +
                      "Инженер уже в пути, ориентировочное время прибытия — 12 мин. " +
                      "Рекомендация: проверить систему охлаждения и очистить " +
                      "вентиляционные отверстия. Пожалуйста, не выключайте АРМ " +
                      "и ожидайте инженера.");
}
