namespace ItSupport.LspIspDip;

/// <summary>
/// Базовый тип АРМ. Любой производный класс можно использовать везде,
/// где ожидается <see cref="Workstation"/>: в инвентаризации, диагностике,
/// удалённом устранении и построении маршрута. Это и есть принцип LSP:
/// подстановка производного типа не ломает программу.
/// </summary>
public abstract class Workstation
{
    protected Workstation(
        string id,
        string name,
        string room,
        string employee,
        Location location)
    {
        Id = id;
        Name = name;
        Room = room;
        Employee = employee;
        Location = location;
    }

    public string Id { get; }
    public string Name { get; }
    public string Room { get; }
    public string Employee { get; }
    public Location Location { get; }

    /// <summary>Категория оборудования для карточки инвентаризации.</summary>
    public abstract string Category { get; }

    /// <summary>
    /// Общая карточка АРМ. Производные классы могут дополнять её,
    /// но не должны выбрасывать исключения, удалять базовые сведения
    /// или иным образом сужать контракт (LSP).
    /// </summary>
    public virtual string GetInventoryCard()
        => $"{Id} — {Name}, {Room}; сотрудник: {Employee}";
}

public sealed class AccountantWorkstation : Workstation
{
    public AccountantWorkstation(
        string id,
        string name,
        string room,
        string employee,
        Location location)
        : base(id, name, room, employee, location)
    {
    }

    public override string Category => "АРМ бухгалтера";
}

public sealed class ManagerWorkstation : Workstation
{
    public ManagerWorkstation(
        string id,
        string name,
        string room,
        string employee,
        Location location)
        : base(id, name, room, employee, location)
    {
    }

    public override string Category => "АРМ менеджера";
}

public sealed class WarehouseWorkstation : Workstation
{
    public WarehouseWorkstation(
        string id,
        string name,
        string room,
        string employee,
        Location location)
        : base(id, name, room, employee, location)
    {
    }

    public override string Category => "АРМ кладовщика";

    /// <summary>
    /// Расширяет базовую карточку, не нарушая её контракт:
    /// вызывающий код получает как минимум те же сведения, что и для Workstation.
    /// </summary>
    public override string GetInventoryCard()
        => base.GetInventoryCard() + "; оборудование: сканер штрихкодов";
}

public sealed class DispatcherWorkstation : Workstation
{
    public DispatcherWorkstation(
        string id,
        string name,
        string room,
        string employee,
        Location location)
        : base(id, name, room, employee, location)
    {
    }

    public override string Category => "АРМ диспетчера";

    public override string GetInventoryCard()
        => base.GetInventoryCard() + "; оборудование: дополнительный монитор";
}
