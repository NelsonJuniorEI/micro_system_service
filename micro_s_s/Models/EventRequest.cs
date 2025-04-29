namespace Event.Models;

public class EventRequest
{
    public required string Name_event { get; set; }
    public required string Type_event { get; set; }
    public int Value_event { get; set; }
    public string Location_event { get; set; }
    public DateTime Date_event { get; set; }

    public List<Field> extra_fields { get; set; }
}

public class Field
{
    public Guid Id { get; set; }
    public string Value { get; set; }
}
