namespace Shared.Models;

public class EventLog
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string EventType { get; set; }
    public string Data { get; set; }
}