using Event.Models;

namespace micro_s_s.Models
{
    public class RegistrationRequest
    {
        public Guid Id_Event { get; set; }

        public string Registration_Name { get; set; }

        public string Registration_Email { get; set; }

        public string WhatsApp { get; set; }
        public List<Field> Fields { get; set; }
    }
}