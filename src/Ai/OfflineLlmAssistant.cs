namespace ItSupport.Ai;

/// <summary>
/// Офлайн-заглушка помощника. Не обращается к сети и не требует API-ключа (использует шаблоны)
/// </summary>
public sealed class OfflineLlmAssistant : ILlmAssistant
{
    public string ComposeEmployeeReply(EmployeeReplyContext context)
    {
        var ticket = context.Ticket;
        var fault = context.Fault;
        var result = context.Result;
        var greeting = $"Здравствуйте, {ticket.Employee}!";

        if (result.IsResolved)
        {
            return $"{greeting} {result.Summary} " +
                   $"Пожалуйста, проверьте работу и подтвердите закрытие заявки {ticket.Number}.";
        }

        var arrival = result.Route is null
            ? "Инженер находится в том же здании и скоро подойдёт."
            : $"Инженер уже в пути, ориентировочное время прибытия — {result.Route.DurationMinutes} мин.";

        return $"{greeting} По заявке {ticket.Number} зарегистрирована неисправность «{fault.Title}». " +
               $"{result.Summary} {arrival} Пожалуйста, не выключайте АРМ и ожидайте инженера.";
    }
}
