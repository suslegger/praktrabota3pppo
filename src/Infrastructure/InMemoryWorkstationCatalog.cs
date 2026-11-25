namespace ItSupport.LspIspDip;

/// <summary>Справочник АРМ в памяти.</summary>
public sealed class InMemoryWorkstationCatalog : IWorkstationCatalog
{
    private readonly IReadOnlyList<Workstation> _workstations;

    public InMemoryWorkstationCatalog(IEnumerable<Workstation> workstations)
    {
        _workstations = workstations.ToList();
    }

    public IReadOnlyList<Workstation> GetAll() => _workstations;

    public Workstation GetById(string id)
        => _workstations.FirstOrDefault(workstation => workstation.Id == id)
           ?? throw new KeyNotFoundException($"АРМ {id} не найдено в инвентаризации.");
}
