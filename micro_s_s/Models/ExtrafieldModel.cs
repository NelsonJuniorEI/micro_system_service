namespace micro_s_s.Models
{
    public class ExtrafieldModel
    {
        public ExtrafieldModel()
        {
            Id = Guid.NewGuid();
        }
        public ExtrafieldModel(string otherfields, Guid id_Event) : this()
        {
            if (Id_Event == null)
                throw new ArgumentException("O valor Id_Event não pode ser nulo ou inválido.");

            OtherFields = otherfields;
            Id_Event = id_Event;
        }
        public void ChangeOtherFields(string otherfields)
        {
            OtherFields = otherfields;
        }

        public Guid Id { get; init; }
        public Guid Id_Event { get; set; }
        public string OtherFields { get; set; }
    }
}