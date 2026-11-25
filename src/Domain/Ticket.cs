namespace ItSupport.LspIspDip;

/// <summary>Статусы заявки.</summary>
public enum TicketStatus
{
    New,
    InProgress,
    Resolved,
    Escalated
}

/// <summary>Заявка сотрудника и допустимые переходы её состояния.</summary>
public sealed class Ticket
{
    public string Number { get; }
    public string WorkstationId { get; }
    public string Employee { get; }
    public string Description { get; }
    public TicketStatus Status { get; private set; } = TicketStatus.New;
    public string? Resolution { get; private set; }

    public Ticket(string number, string workstationId, string employee, string description)
    {
        Number = number;
        WorkstationId = workstationId;
        Employee = employee;
        Description = description;
    }

    public void Accept()
    {
        if (Status != TicketStatus.New)
        {
            throw new InvalidOperationException(
                $"Заявку {Number} нельзя принять: текущий статус — {Status}.");
        }

        Status = TicketStatus.InProgress;
    }

    public void Resolve(string resolution)
    {
        EnsureInProgress();
        Resolution = resolution;
        Status = TicketStatus.Resolved;
    }

    public void Escalate(string resolution)
    {
        EnsureInProgress();
        Resolution = resolution;
        Status = TicketStatus.Escalated;
    }

    private void EnsureInProgress()
    {
        if (Status != TicketStatus.InProgress)
        {
            throw new InvalidOperationException(
                $"Заявка {Number} должна быть принята в работу (сейчас — {Status}).");
        }
    }
}
