namespace ItSupport.Domain;

/// <summary>Автоматизированное рабочее место (АРМ) из модуля инвентаризации.</summary>
public sealed record Workstation(
    string Id,
    string Name,
    string Room,
    string Employee,
    Location Location);
