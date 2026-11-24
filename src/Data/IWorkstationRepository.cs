using ItSupport.Domain;

namespace ItSupport.Data;

/// <summary>Доступ к справочнику оборудования (модуль инвентаризации).</summary>
public interface IWorkstationRepository
{
    IReadOnlyList<Workstation> GetAll();

    /// <exception cref="KeyNotFoundException">АРМ с указанным идентификатором не найдено.</exception>
    Workstation GetById(string id);
}
