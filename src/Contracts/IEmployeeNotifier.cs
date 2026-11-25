namespace ItSupport.LspIspDip;

/// <summary>Канал уведомления сотрудника. Не зависит от способа доставки.</summary>
public interface IEmployeeNotifier
{
    void Send(string employee, string message);
}
