using ItSupport.Domain;

namespace ItSupport.Remediation;

/// <summary>Все данные, необходимые стратегии для устранения неисправности.</summary>
public sealed record RemediationContext(
    Ticket Ticket,
    Workstation Workstation,
    Operator Operator,
    Fault Fault);
