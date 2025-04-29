using System.ComponentModel.DataAnnotations;

namespace micro_s_s.Models
{
    public class FieldanswerModel
    {
        public FieldanswerModel()
        {
            Id = Guid.NewGuid();
        }

        public FieldanswerModel(Guid id_fields, Guid id_Registered, string answer)
        {
            Answer = answer;
            Id_Fields = id_fields;
            Id_Registered = id_Registered;
        }

        [Key]
        public Guid Id { get; set; }
        public Guid Id_Fields { get; set; }
        public Guid Id_Registered { get; set; }
        public string Answer { get; set; }
    
        public void ChangeAnswer(string answer) => Answer = answer;
    }
}