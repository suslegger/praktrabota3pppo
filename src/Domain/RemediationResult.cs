namespace ItSupport.LspIspDip;

/// <summary>Данные, передаваемые стратегии устранения.</summary>
public sealed record RemediationContext(
    Ticket Ticket,
    Workstation Workstation,
    Operator Operator,
    Fault Fault);

/// <summary>Результат устранения неисправности.</summary>
public sealed record RemediationResult(
    bool IsResolved,
    string Summary,
    Route? Route,
    IReadOnlyList<string> Log);
