using ItSupport.Domain;
using ItSupport.Remediation;

namespace ItSupport.Ai;

/// <summary>Данные, которые передаются помощнику для составления ответа сотруднику.</summary>
public sealed record EmployeeReplyContext(
    Ticket Ticket,
    Workstation Workstation,
    Fault Fault,
    RemediationResult Result);
