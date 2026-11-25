namespace ItSupport.LspIspDip;

/// <summary>Доступ к справочнику оборудования (модуль инвентаризации).</summary>
public interface IWorkstationCatalog
{
    IReadOnlyList<Workstation> GetAll();

    /// <exception cref="KeyNotFoundException">АРМ с указанным идентификатором не найдено.</exception>
    Workstation GetById(string id);
}
