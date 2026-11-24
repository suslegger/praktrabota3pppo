using ItSupport.Domain;

namespace ItSupport.Data;

/// <summary>Хранилище заявок технической поддержки.</summary>
public interface ITicketRepository
{
    IReadOnlyList<Ticket> GetAll();

    void Save(Ticket ticket);
}
