namespace ItSupport.BuilderReport;

/// <summary>
/// Конкретный строитель: собирает обычный текстовый отчёт.
/// </summary>
public sealed class TextReportBuilder : ReportBuilder
{
    public override void BuildHeader(IncidentData data)
    {
        AppendLine(new string('=', 62));
        AppendLine($"ОТЧЁТ ОБ ИНЦИДЕНТЕ {data.TicketNumber}");
        AppendLine(new string('=', 62));
        AppendLine($"Сформирован: {DateTime.Now:dd.MM.yyyy HH:mm}");
        AppendLine();
    }

    public override void BuildEquipment(IncidentData data)
    {
        AppendLine("ОБОРУДОВАНИЕ");
        AppendLine($"  АРМ:        {data.WorkstationId} — {data.WorkstationName}");
        AppendLine($"  Кабинет:    {data.Room}");
        AppendLine($"  Адрес:      {data.Address}");
        AppendLine($"  Сотрудник:  {data.Employee}");
        AppendLine();
    }

    public override void BuildTelemetry(IncidentData data)
    {
        var telemetry = data.Telemetry;

        AppendLine("ТЕЛЕМЕТРИЯ");
        AppendLine($"  Свободно на диске: {telemetry.DiskFreePercent:F0}%");
        AppendLine($"  Сеть:              {(telemetry.NetworkAvailable ? "доступна" : "нет")}");
        AppendLine($"  Температура:       {telemetry.CpuTemperature:F0} °C");
        AppendLine($"  Питание:           {(telemetry.PowerSupplyOk ? "норма" : "сбой")}");
        AppendLine();
    }

    public override void BuildDiagnostics(IncidentData data)
    {
        var fault = data.Fault;

        AppendLine("ДИАГНОСТИКА");
        AppendLine($"  Код:                    {fault.Code}");
        AppendLine($"  Неисправность:          {fault.Title}");
        AppendLine($"  Подробности:            {fault.Details}");
        AppendLine($"  Удалённое устранение:   {(fault.CanBeFixedRemotely ? "возможно" : "невозможно")}");
        AppendLine();
    }

    public override void BuildRemediation(IncidentData data)
    {
        var remediation = data.Remediation;

        AppendLine("УСТРАНЕНИЕ");
        AppendLine($"  Стратегия:  {remediation.Strategy}");
        AppendLine($"  Решение:    {remediation.Summary}");

        if (remediation.RouteDistanceKm is not null && remediation.RouteMinutes is not null)
        {
            AppendLine($"  Маршрут:    {remediation.RouteDistanceKm:F1} км, " +
                       $"{remediation.RouteMinutes} мин");
        }

        AppendLine();
    }

    public override void BuildLlmReply(IncidentData data)
    {
        AppendLine("ОТВЕТ СОТРУДНИКУ (LLM)");
        AppendLine($"  {data.LlmReply}");
    }

    public override IncidentReport GetResult()
        => new("TEXT", Content.ToString());
}
