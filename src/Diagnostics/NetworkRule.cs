using ItSupport.Domain;

namespace ItSupport.Diagnostics;

/// <summary>Правило: сетевое подключение АРМ недоступно.</summary>
public sealed class NetworkRule : IFaultDiagnosticRule
{
    public Fault? Inspect(Telemetry telemetry)
    {
        if (telemetry.NetworkAvailable)
        {
            return null;
        }

        return new Fault(
            Code: "NO_NETWORK",
            Title: "Отсутствует сеть",
            Details: "Сетевой адаптер не отвечает, адрес шлюза недоступен.",
            CanBeFixedRemotely: true);
    }
}
