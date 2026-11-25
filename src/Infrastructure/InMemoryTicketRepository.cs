namespace ItSupport.LspIspDip;

/// <summary>Хранилище заявок в памяти.</summary>
public sealed class InMemoryTicketRepository : ITicketRepository
{
    private readonly List<Ticket> _tickets;

    public InMemoryTicketRepository(IEnumerable<Ticket> tickets)
    {
        _tickets = tickets.ToList();
    }

    public IReadOnlyList<Ticket> GetAll() => _tickets;

    public void Save(Ticket ticket)
    {
        var index = _tickets.FindIndex(existing => existing.Number == ticket.Number);
        if (index >= 0)
        {
            _tickets[index] = ticket;
        }
        else
        {
            _tickets.Add(ticket);
        }
    }
}
