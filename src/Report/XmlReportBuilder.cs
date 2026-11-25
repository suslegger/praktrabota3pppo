using System.Globalization;

namespace ItSupport.BuilderReport;

/// <summary>
/// Конкретный строитель: собирает XML-представление отчёта.
/// </summary>
public sealed class XmlReportBuilder : ReportBuilder
{
    public override void BuildHeader(IncidentData data)
    {
        AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
        AppendLine($"<incidentReport ticket=\"{Escape(data.TicketNumber)}\">");
        AppendLine("  <header>");
        AppendLine($"    <number>{Escape(data.TicketNumber)}</number>");
        AppendLine($"    <generatedAt>{DateTime.Now:O}</generatedAt>");
        AppendLine("  </header>");
    }

    public override void BuildEquipment(IncidentData data)
    {
        AppendLine("  <equipment>");
        AppendLine($"    <workstation id=\"{Escape(data.WorkstationId)}\" " +
                   $"name=\"{Escape(data.WorkstationName)}\" />");
        AppendLine($"    <room>{Escape(data.Room)}</room>");
        AppendLine($"    <address>{Escape(data.Address)}</address>");
        AppendLine($"    <employee>{Escape(data.Employee)}</employee>");
        AppendLine("  </equipment>");
    }

    public override void BuildTelemetry(IncidentData data)
    {
        var telemetry = data.Telemetry;

        AppendLine("  <telemetry>");
        AppendLine($"    <diskFreePercent>{telemetry.DiskFreePercent.ToString(CultureInfo.InvariantCulture)}" +
                   "</diskFreePercent>");
        AppendLine($"    <networkAvailable>{telemetry.NetworkAvailable.ToString().ToLowerInvariant()}" +
                   "</networkAvailable>");
        AppendLine($"    <cpuTemperature>{telemetry.CpuTemperature.ToString(CultureInfo.InvariantCulture)}" +
                   "</cpuTemperature>");
        AppendLine($"    <powerSupplyOk>{telemetry.PowerSupplyOk.ToString().ToLowerInvariant()}" +
                   "</powerSupplyOk>");
        AppendLine("  </telemetry>");
    }

    public override void BuildDiagnostics(IncidentData data)
    {
        var fault = data.Fault;

        AppendLine("  <diagnostics>");
        AppendLine($"    <code>{Escape(fault.Code)}</code>");
        AppendLine($"    <title>{Escape(fault.Title)}</title>");
        AppendLine($"    <details>{Escape(fault.Details)}</details>");
        AppendLine($"    <canBeFixedRemotely>{fault.CanBeFixedRemotely.ToString().ToLowerInvariant()}" +
                   "</canBeFixedRemotely>");
        AppendLine("  </diagnostics>");
    }

    public override void BuildRemediation(IncidentData data)
    {
        var remediation = data.Remediation;

        AppendLine("  <remediation>");
        AppendLine($"    <strategy>{Escape(remediation.Strategy)}</strategy>");
        AppendLine($"    <summary>{Escape(remediation.Summary)}</summary>");

        if (remediation.RouteDistanceKm is not null)
        {
            var distance = remediation.RouteDistanceKm.Value
                .ToString("F1", CultureInfo.InvariantCulture);
            AppendLine($"    <routeDistanceKm>{distance}</routeDistanceKm>");
        }

        if (remediation.RouteMinutes is not null)
        {
            AppendLine($"    <routeMinutes>{remediation.RouteMinutes.Value}</routeMinutes>");
        }

        AppendLine("  </remediation>");
    }

    public override void BuildLlmReply(IncidentData data)
    {
        AppendLine($"  <llmReply>{Escape(data.LlmReply)}</llmReply>");
    }

    public override IncidentReport GetResult()
    {
        AppendLine("</incidentReport>");
        return new IncidentReport("XML", Content.ToString());
    }

    private static string Escape(string value)
        => value
            .Replace("&", "&amp;")
            .Replace("<", "&lt;")
            .Replace(">", "&gt;")
            .Replace("\"", "&quot;")
            .Replace("'", "&apos;");
}
