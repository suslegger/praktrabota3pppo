using ItSupport.Domain;

namespace ItSupport.Data;

/// <summary>Справочник АРМ в памяти для прототипа без базы данных.</summary>
public sealed class InMemoryWorkstationRepository : IWorkstationRepository
{
    private readonly IReadOnlyList<Workstation> _workstations;

    public InMemoryWorkstationRepository(IEnumerable<Workstation> workstations)
    {
        _workstations = workstations.ToList();
    }

    public IReadOnlyList<Workstation> GetAll() => _workstations;

    public Workstation GetById(string id)
        => _workstations.FirstOrDefault(workstation => workstation.Id == id)
           ?? throw new KeyNotFoundException($"АРМ {id} не найдено в инвентаризации.");
}
