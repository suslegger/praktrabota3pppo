namespace ItSupport.LspIspDip;

/// <summary>
/// Офлайн-заглушка LLM. Формирует ответ по шаблону, используя полиморфный
/// метод <see cref="Fault.GetRecommendation"/>.
/// </summary>
public sealed class OfflineLlmAssistant : ILlmAssistant
{
    public string ComposeReply(ReplyContext context)
    {
        var greeting = $"Здравствуйте, {context.Ticket.Employee}!";
        var recommendation = context.Fault.GetRecommendation();
        var recommendationText = "Рекомендация: " +
            char.ToLowerInvariant(recommendation[0]) + recommendation[1..];

        if (context.Result.IsResolved)
        {
            return $"{greeting} {context.Result.Summary} {recommendationText} " +
                   $"Пожалуйста, проверьте работу и подтвердите закрытие заявки " +
                   $"{context.Ticket.Number}.";
        }

        var arrival = context.Result.Route is null
            ? "Инженер находится в том же здании и скоро подойдёт."
            : $"Инженер уже в пути, ориентировочное время прибытия — " +
              $"{context.Result.Route.DurationMinutes} мин.";

        return $"{greeting} По заявке {context.Ticket.Number} зарегистрирована " +
               $"неисправность «{context.Fault.Title}». {context.Result.Summary} " +
               $"{arrival} {recommendationText} Пожалуйста, не выключайте АРМ " +
               "и ожидайте инженера.";
    }
}
