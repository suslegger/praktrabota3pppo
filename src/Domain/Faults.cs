namespace ItSupport.LspIspDip;

/// <summary>
/// Базовый тип неисправности. Диагностика возвращает <see cref="Fault"/>,
/// а подсистема устранения принимает <see cref="Fault"/>; конкретный тип
/// неисправности нигде не проверяется через is/as. Любой производный тип
/// подставляется без изменения поведения клиента (LSP).
/// </summary>
public abstract class Fault
{
    protected Fault(string code, string title, string details)
    {
        Code = code;
        Title = title;
        Details = details;
    }

    public string Code { get; }
    public string Title { get; }
    public string Details { get; }

    /// <summary>Можно ли устранить неисправность без выезда инженера.</summary>
    public abstract bool CanBeFixedRemotely { get; }

    /// <summary>Рекомендация по устранению или профилактике.</summary>
    public virtual string GetRecommendation()
        => $"Устранить неисправность «{Title}».";
}

public sealed class DiskFullFault : Fault
{
    public DiskFullFault(double freePercent)
        : base(
            "DISK_FULL",
            "Недостаточно места на диске",
            $"Свободно {freePercent:F1}% (порог — 5,0%).")
    {
    }

    public override bool CanBeFixedRemotely => true;

    public override string GetRecommendation()
        => "Очистить временные файлы и архив.";
}

public sealed class NetworkFault : Fault
{
    public NetworkFault()
        : base(
            "NO_NETWORK",
            "Отсутствует сеть",
            "Сетевой адаптер не отвечает, адрес шлюза недоступен.")
    {
    }

    public override bool CanBeFixedRemotely => true;

    public override string GetRecommendation()
        => "Проверить кабель и перезапустить сетевой адаптер.";
}

public sealed class OverheatFault : Fault
{
    public OverheatFault(double temperature)
        : base(
            "OVERHEAT",
            "Перегрев процессора",
            $"Температура {temperature:F0} °C (порог — 85 °C).")
    {
    }

    public override bool CanBeFixedRemotely => false;

    public override string GetRecommendation()
        => "Проверить систему охлаждения и очистить вентиляционные отверстия.";
}

public sealed class PowerSupplyFault : Fault
{
    public PowerSupplyFault()
        : base(
            "PSU_FAILURE",
            "Неисправность блока питания",
            "Питание нестабильно, требуется осмотр оборудования.")
    {
    }

    public override bool CanBeFixedRemotely => false;

    public override string GetRecommendation()
        => "Заменить блок питания после диагностики на месте.";
}
