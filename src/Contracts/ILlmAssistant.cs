namespace ItSupport.LspIspDip;

/// <summary>Контекст для формирования ответа сотруднику.</summary>
public sealed record ReplyContext(
    Ticket Ticket,
    Workstation Workstation,
    Fault Fault,
    RemediationResult Result);

/// <summary>
/// Помощник, формирующий ответ сотруднику. Реальная LLM подключается
/// отдельной реализацией; высокоуровневый сценарий от этого не меняется.
/// </summary>
public interface ILlmAssistant
{
    string ComposeReply(ReplyContext context);
}
