# Практическая работа № 5. Паттерн «Строитель» (Builder)

Программа по тому же варианту, что и предыдущие работы: интеллектуальная
платформа сопровождения ИТ-инфраструктуры. В качестве паттерна выбран
**Строитель (Builder)**: он позволяет пошагово собирать сложный объект —
отчёт об инциденте — и получать разные представления одного и того же отчёта.

## Что делает программа

1. Формирует снимок инцидента `IncidentData` по заявке TCK-1003 (перегрев
   процессора на АРМ WS-310).
2. Создаёт распорядителя `ReportDirector` с конкретным строителем.
3. Директор последовательно вызывает шаги построения:
   - заголовок;
   - оборудование;
   - телеметрия;
   - диагностика;
   - устранение;
   - ответ сотруднику.
4. Получает готовый продукт `IncidentReport`:
   - в текстовом виде (`TextReportBuilder`);
   - в XML-виде (`XmlReportBuilder`).
5. Выводит оба отчёта в консоль и сохраняет их в папку `output/`.

## Запуск

Из корня репозитория:

```powershell
dotnet run --project .\src\ItSupport.BuilderReport.csproj
```

После запуска создаются:

- `output/incident-report.txt` — текстовый отчёт;
- `output/incident-report.xml` — XML-отчёт.

## Участники паттерна

| Роль | Класс | Назначение |
|---|---|---|
| Product | `IncidentReport` | готовый отчёт, содержащий формат и текст |
| Builder | `ReportBuilder` | абстрактные шаги построения отчёта |
| ConcreteBuilder | `TextReportBuilder` | собирает текстовое представление |
| ConcreteBuilder | `XmlReportBuilder` | собирает XML-представление |
| Director | `ReportDirector` | задаёт порядок шагов построения |
| Client | `Program` | выбирает строителя и запускает директора |

## Структура

```
README.md
diagram/
   ├── builder-class-diagram.drawio
   └── builder-class-diagram.drawio.svg
src/
   ├── Domain/
   │   └── IncidentData.cs      # входные данные об инциденте
   ├── Report/
   │   ├── IncidentReport.cs    # продукт
   │   ├── ReportBuilder.cs     # абстрактный строитель
   │   ├── TextReportBuilder.cs # текстовый строитель
   │   ├── XmlReportBuilder.cs  # XML-строитель
   │   └── ReportDirector.cs    # распорядитель
   ├── Program.cs               # клиент
   └── ItSupport.BuilderReport.csproj
```

## Диаграмма классов

Диаграмма построена для паттерна Строитель и находится в папке `diagram/`:

- `builder-class-diagram.drawio` — редактируемый файл draw.io;
- `builder-class-diagram.drawio.svg` — векторное изображение.

На диаграмме показаны `Program` (Client), `ReportDirector` (Director),
`ReportBuilder` (Builder), два конкретных строителя, продукт `IncidentReport`
и входные данные `IncidentData`. Для компактности у конкретных строителей
список методов сокращён до `+Build...()`; в коде они реализуют все шаги
`BuildHeader`, `BuildEquipment`, `BuildTelemetry`, `BuildDiagnostics`,
`BuildRemediation` и `BuildLlmReply`.
