namespace ItSupport.LspIspDip;

/// <summary>Консольное меню оператора.</summary>
public sealed class SupportConsoleApp
{
    private readonly ITicketRepository _tickets;
    private readonly IncidentService _incidents;
    private readonly IUserInterface _ui;
    private readonly Operator _operator;

    public SupportConsoleApp(
        ITicketRepository tickets,
        IncidentService incidents,
        IUserInterface ui,
        Operator operatorUser)
    {
        _tickets = tickets;
        _incidents = incidents;
        _ui = ui;
        _operator = operatorUser;
    }

    public void Run()
    {
        while (true)
        {
            ShowTicketList();

            var input = _ui.Ask("Выберите заявку: ");
            if (input.Trim() == "0")
            {
                return;
            }

            var tickets = _tickets.GetAll();
            if (!int.TryParse(input, out var number) || number < 1 || number > tickets.Count)
            {
                _ui.Error("Некорректный номер заявки.");
                _ui.Pause();
                continue;
            }

            var ticket = tickets[number - 1];
            if (ticket.Status != TicketStatus.New)
            {
                _ui.Warning($"Заявка {ticket.Number} уже обработана (статус: {ticket.Status}).");
                _ui.Pause();
                continue;
            }

            _incidents.Process(ticket);
            _ui.Pause();
        }
    }

    private void ShowTicketList()
    {
        _ui.Clear();
        _ui.Header("Интеллектуальная платформа сопровождения ИТ-инфраструктуры");
        _ui.Info($"Оператор: {_operator.Name} ({_operator.Location.Address})");
        _ui.Info(string.Empty);
        _ui.Info("Заявки:");

        var tickets = _tickets.GetAll();
        for (var index = 0; index < tickets.Count; index++)
        {
            var ticket = tickets[index];
            _ui.Info($"{index + 1}. [{ticket.Status}] {ticket.Number} — " +
                     $"{ticket.WorkstationId}: {ticket.Description}");
        }

        _ui.Info("0. Выход");
        _ui.Info(string.Empty);
    }
}
