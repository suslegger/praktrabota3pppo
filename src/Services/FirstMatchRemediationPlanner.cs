namespace ItSupport.LspIspDip;

/// <summary>Выбирает первую стратегию, которая умеет обработать неисправность.</summary>
public sealed class FirstMatchRemediationPlanner : IRemediationPlanner
{
    private readonly IReadOnlyList<Remediation> _remediations;

    public FirstMatchRemediationPlanner(IEnumerable<Remediation> remediations)
    {
        _remediations = remediations.ToList();
    }

    public Remediation Select(Fault fault)
        => _remediations.FirstOrDefault(remediation => remediation.CanHandle(fault))
           ?? throw new InvalidOperationException(
               $"Для неисправности {fault.Code} не зарегистрирована стратегия устранения.");
}
