namespace micro_system_service.Models
{
    public class EventsModel
    {
        // Construtor sem parâmetros (necessário para o EF Core)
        public EventsModel(string name_event, string type_event)
        {
            Id = Guid.NewGuid();  // Garantir que o Id seja atribuído automaticamente
            Name_event = name_event;
            Type_event = type_event;
        }

        // Construtor com parâmetros (pode ser utilizado manualmente)
        public EventsModel(string name, string type, int value, string location, DateTime date)
        {
            Name_event = name;
            Type_event = type;
            Value_event = value;
            Location_event = location;
            Date_event = date;
        }

        public Guid Id { get; init; } // 'init' só permite alteração do valor na criação do objeto
        public string Name_event { get; private set; }
        public string Type_event { get; set; }
        public int Value_event { get; set; }
        public string Location_event { get; set; }
        public DateTime Date_event { get; set; }

        public void ChangeAllData(string name, string type, int value, string location, DateTime date)
        {
            (Name_event, Type_event, Value_event, Location_event, Date_event) = (name, type, value, location, date);
        }

        public void ChangeName(string name)
        {
            Name_event = name;
        }

        public void ChangeTypeDelete()
        {
            Type_event = "desativado";
        }
    }
}