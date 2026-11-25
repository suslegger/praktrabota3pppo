# Вариант с принципами LSP, ISP и DIP

Третий вариант прототипа платформы сопровождения ИТ-инфраструктуры. Сценарий
тот же, что в варианте `beginner`: оператор принимает заявку, система читает
телеметрию АРМ, диагностирует неисправность, устраняет её удалённо или
организует выезд с маршрутом, после чего формирует ответ сотруднику.

В этом варианте применены принципы практической работы № 4:

| Принцип | Где проявляется |
|---|---|
| **LSP** — подстановка Лисков | иерархии `Workstation`, `Fault`, `Remediation` |
| **ISP** — разделение интерфейсов | набор узких интерфейсов в `Contracts/` |
| **DIP** — инверсия зависимостей | `IncidentService` зависит от абстракций, `Program` — точка сборки |

## Запуск

Из корня репозитория:

```powershell
dotnet run --project .\src\ItSupport.LspIspDip.csproj
```

Требуется .NET SDK 8.0 или новее. Внешние сервисы и API-ключи не нужны:
телеметрия, удалённые сеансы, маршрут и LLM имитируются.

## Демонстрационные заявки

| № | Заявка | АРМ | Неисправность | Результат |
|---|---|---|---|---|
| 1 | TCK-1001 | WS-101 | мало места на диске | удалённое устранение |
| 2 | TCK-1002 | WS-205 | нет сети | удалённое устранение |
| 3 | TCK-1003 | WS-310 | перегрев процессора | выезд, маршрут 3,7 км |
| 4 | TCK-1004 | WS-415 | сбой питания | выезд, АРМ в том же здании |

## Как принципы видны в коде

### LSP

- `Domain/Workstations.cs` — базовый `Workstation` и четыре производных типа
  (`AccountantWorkstation`, `ManagerWorkstation`, `WarehouseWorkstation`,
  `DispatcherWorkstation`). Производные классы дополняют карточку оборудования,
  но не ломают базовый контракт.
- `Domain/Faults.cs` — базовый `Fault` и конкретные неисправности. Диагностика
  и устранение работают с `Fault`, не проверяя конкретный тип.
- `Services/Remediation.cs`, `Services/RemoteRemediation.cs` — базовый
  `Remediation` и стратегии. Планировщик вызывает методы базового класса,
  поэтому любая корректная стратегия подставляется без его изменения.

### ISP

В `Contracts/` нет «жирного» интерфейса платформы. Вместо него:

- `IWorkstationCatalog` — только инвентаризация;
- `ITelemetrySource` — только телеметрия;
- `IRemoteSessionFactory` и `IRemoteSession` — только удалённые сеансы;
- `IRoutePlanner` — только маршрут;
- `ITicketRepository` — только заявки;
- `IEmployeeNotifier` — только уведомления;
- `ILlmAssistant` — только ответ сотруднику;
- `IDiagnosticsService`, `IRemediationPlanner`, `IUserInterface` — свои роли.

`RemoteRemediation` зависит только от `IRemoteSessionFactory`, а
`OnsiteVisitRemediation` — только от `IRoutePlanner`. Им не нужны чужие методы.

### DIP

`Services/IncidentService.cs` — высокоуровневый сценарий — не создаёт
конкретные классы и не знает про `Console`, `DemoTelemetrySource` или
`ConsoleRemoteSession`. Все зависимости передаются через конструктор:

```csharp
public IncidentService(
    IWorkstationCatalog catalog,
    ITelemetrySource telemetrySource,
    IDiagnosticsService diagnostics,
    IRemediationPlanner remediationPlanner,
    ITicketRepository tickets,
    ILlmAssistant llm,
    IEmployeeNotifier notifier,
    IUserInterface ui,
    Operator operatorUser)
```

Конкретные реализации собираются в `Program.cs`:

```csharp
var incidents = new IncidentService(
    catalog,
    telemetry,
    diagnostics,
    planner,
    tickets,
    new OfflineLlmAssistant(),
    new ConsoleEmployeeNotifier(),
    ui,
    operatorUser);
```

## Структура

```
src/
├── Domain/           модели, иерархии Workstation и Fault
├── Contracts/        узкие интерфейсы (порты)
├── Services/         диагностика, стратегии устранения, сценарий заявки
├── Infrastructure/   консольные и демонстрационные реализации
├── Presentation/     меню оператора
├── Program.cs        точка сборки
└── ItSupport.LspIspDip.csproj
```

## Отличие от других вариантов

| Ветка | Принципы | Особенность |
|---|---|---|
| `beginner/` | нет | один класс, вся логика в одном методе |
| `solid/` | SRP, OCP | правила диагностики и стратегии устранения |
| `lsp-isp-dip/` | LSP, ISP, DIP | иерархии типов, узкие интерфейсы, внедрение зависимостей |
