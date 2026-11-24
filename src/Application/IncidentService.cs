using ItSupport.Ai;
using ItSupport.Data;
using ItSupport.Diagnostics;
using ItSupport.Domain;
using ItSupport.Presentation;
using ItSupport.Remediation;

namespace ItSupport.Application;

/// <summary>
/// Сценарий обработки одной заявки
/// </summary>
public sealed class IncidentService
{
    private readonly IWorkstationRepository _workstations;
    private readonly ITelemetrySource _telemetrySource;
    private readonly FaultDiagnosticsService _diagnostics;
    private readonly RemediationPlanner _planner;
    private readonly ILlmAssistant _llm;
    private readonly ITicketRepository _tickets;
    private readonly IUserInterface _ui;
    private readonly Operator _operator;

    public IncidentService(
        IWorkstationRepository workstations,
        ITelemetrySource telemetrySource,
        FaultDiagnosticsService diagnostics,
        RemediationPlanner planner,
        ILlmAssistant llm,
        ITicketRepository tickets,
        IUserInterface ui,
        Operator operatorUser)
    {
        _workstations = workstations;
        _telemetrySource = telemetrySource;
        _diagnostics = diagnostics;
        _planner = planner;
        _llm = llm;
        _tickets = tickets;
        _ui = ui;
        _operator = operatorUser;
    }

    public void Process(Ticket ticket)
    {
        _ui.Header($"Заявка {ticket.Number}: {ticket.Description}");
        ticket.Accept();
        _ui.Success("[Оператор] Заявка принята в работу.");

        var workstation = _workstations.GetById(ticket.WorkstationId);
        _ui.Info($"[Инвентаризация] {workstation.Id} — {workstation.Name}, " +
                 $"{workstation.Room}; сотрудник: {workstation.Employee}");

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

        // Если есть неисправность, требующая выезда, обрабатываем прежде всего её.
        var primaryFault = faults.FirstOrDefault(fault => !fault.CanBeFixedRemotely) ?? faults[0];
        if (faults.Count > 1)
        {
            _ui.Info($"[Диагностика] Обнаружено несколько неисправностей, " +
                     $"обрабатывается основная: {primaryFault.Code}.");
        }

        _ui.Info($"[Планировщик] Подбираю способ устранения неисправности {primaryFault.Code}...");
        var strategy = _planner.Select(primaryFault);
        _ui.Info($"[Планировщик] Выбрана стратегия: «{strategy.Name}».");

        var result = strategy.Remediate(new RemediationContext(ticket, workstation, _operator, primaryFault));
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

        var reply = _llm.ComposeEmployeeReply(
            new EmployeeReplyContext(ticket, workstation, primaryFault, result));

        _ui.Header("[LLM] Черновик ответа сотруднику");
        _ui.Info(reply);
        _ui.Info("[Оператор] Ответ отправлен сотруднику.");

        PrintStatus(ticket);
    }

    private void PrintStatus(Ticket ticket)
    {
        var status = ticket.Status == TicketStatus.Resolved ? "RESOLVED" : "ESCALATED";
        _ui.Success($"[Статус] {ticket.Number}: {status} — {ticket.Resolution}");
    }
}
