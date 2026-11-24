namespace ItSupport.Ai;

/// <summary>
/// Помощник, формирующий ответ сотруднику по контексту заявки
/// </summary>
public interface ILlmAssistant
{
    string ComposeEmployeeReply(EmployeeReplyContext context);
}
