namespace ItSupport.LspIspDip;

/// <summary>
/// Высокоуровневый сценарий обработки заявки. Класс зависит только от
/// абстракций (DIP): каталога, источника телеметрии, диагностики,
/// планировщика устранения, репозитория, LLM, уведомлений и интерфейса.
/// Конкретные реализации создаются в Program.cs.
/// </summary>
public sealed class IncidentService
{
    private readonly IWorkstationCatalog _catalog;
    private readonly ITelemetrySource _telemetrySource;
    private readonly IDiagnosticsService _diagnostics;
    private readonly IRemediationPlanner _remediationPlanner;
    private readonly ITicketRepository _tickets;
    private readonly ILlmAssistant _llm;
    private readonly IEmployeeNotifier _notifier;
    private readonly IUserInterface _ui;
    private readonly Operator _operator;

    public IncidentService(
        IWorkstationCatalog catalog,
        ITelemetrySource telemetrySource,
        IDiagnosticsService diagnostics,
        IRemediationPlanner remediationPlanner,
        ITicketRepository tickets,
        ILlmAssistant llm,
        IEmployeeNotifier notifier,
        IUserInterface ui,
        Operator operatorUser)
    {
        _catalog = catalog;
        _telemetrySource = telemetrySource;
        _diagnostics = diagnostics;
        _remediationPlanner = remediationPlanner;
        _tickets = tickets;
        _llm = llm;
        _notifier = notifier;
        _ui = ui;
        _operator = operatorUser;
    }

    public void Process(Ticket ticket)
    {
        _ui.Header($"Заявка {ticket.Number}: {ticket.Description}");
        ticket.Accept();
        _ui.Success("[Оператор] Заявка принята в работу.");

        var workstation = _catalog.GetById(ticket.WorkstationId);
        _ui.Info($"[Инвентаризация] {workstation.GetInventoryCard()} " +
                 $"(категория: {workstation.Category}).");

        _ui.Info($"[Мониторинг] Считываю данные с {workstation.Id}...");
        var telemetry = _telemetrySource.Read(workstation.Id);
        _ui.Info($"[Мониторинг] Диск: {telemetry.DiskFreePercent:F0}% свободно; " +
                 $"сеть: {(telemetry.NetworkAvailable ? "доступна" : "нет")}; " +
                 $"температура: {telemetry.CpuTemperature:F0} °C; " +
                 $"питание: {(telemetry.PowerSupplyOk ? "норма" : "сбой")}.");

        var faults = _diagnostics.Diagnose(telemetry);
        if (faults.Count == 0)
        {
            const string message = "Неисправностей не обнаружено — вероятно, проблема уже устранена.";
            _ui.Warning($"[Диагностика] {message}");
            ticket.Resolve(message);
            _tickets.Save(ticket);
            PrintStatus(ticket);
            return;
        }

        foreach (var fault in faults)
        {
            _ui.Warning($"[Диагностика] {fault.Code}: {fault.Title} — {fault.Details}");
        }

        var primaryFault = faults.FirstOrDefault(fault => !fault.CanBeFixedRemotely) ?? faults[0];
        if (faults.Count > 1)
        {
            _ui.Info($"[Диагностика] Обнаружено несколько неисправностей, " +
                     $"обрабатывается основная: {primaryFault.Code}.");
        }

        _ui.Info($"[Планировщик] Подбираю способ устранения неисправности {primaryFault.Code}...");
        var remediation = _remediationPlanner.Select(primaryFault);
        _ui.Info($"[Планировщик] Выбрана стратегия: «{remediation.Name}».");

        var result = remediation.Execute(
            new RemediationContext(ticket, workstation, _operator, primaryFault));

        foreach (var line in result.Log)
        {
            _ui.Info(line);
        }

        if (result.IsResolved)
        {
            ticket.Resolve(result.Summary);
        }
        else
        {
            ticket.Escalate(result.Summary);
        }

        _tickets.Save(ticket);

        var reply = _llm.ComposeReply(
            new ReplyContext(ticket, workstation, primaryFault, result));

        _ui.Header("[LLM] Черновик ответа сотруднику");
        _ui.Info(reply);
        _notifier.Send(ticket.Employee, reply);

        PrintStatus(ticket);
    }

    private void PrintStatus(Ticket ticket)
    {
        var status = ticket.Status == TicketStatus.Resolved ? "RESOLVED" : "ESCALATED";
        _ui.Success($"[Статус] {ticket.Number}: {status} — {ticket.Resolution}");
    }
}
