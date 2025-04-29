
namespace micro_s_s.Models
{
    public class RegistrationModel
    {
        public RegistrationModel()
        {
            Id = Guid.NewGuid();
        }

        public RegistrationModel(string name, string email, string whatsapp, Guid id_event)
        {
            Registration_Name = name;
            Registration_Email = email;
            WhatsApp = whatsapp;
            Id_Event = id_event;
        }

        public void ChangeR_Name(string name)
        {
            Registration_Name = name;
        }
        public void ChangeEmail(string email)
        {
            Registration_Email = email;
        }
        public void ChangeWhatsApp(string whatsapp)
        {
            WhatsApp = whatsapp;
        }
        public void SetInactive()
        {
            Registration_Name = "desativado";
        }

        public Guid Id { get; init; }
        public Guid Id_Event { get; set; }
        public string Registration_Name { get; set; }
        public string Registration_Email { get; set; }
        public string WhatsApp { get; set; }
    }
}