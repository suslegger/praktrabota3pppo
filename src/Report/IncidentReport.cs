namespace ItSupport.BuilderReport;

/// <summary>
/// Продукт паттерна Строитель — готовый отчёт об инциденте.
/// Содержимое уже сформировано конкретным строителем.
/// </summary>
public sealed class IncidentReport
{
    public string Format { get; }
    public string Content { get; }

    public IncidentReport(string format, string content)
    {
        Format = format;
        Content = content;
    }

    public void Print()
    {
        Console.WriteLine(Content);
    }
}
