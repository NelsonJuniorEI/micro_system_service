using micro_s_s.Models;
using micro_system_service.Models;
using Microsoft.EntityFrameworkCore;

namespace micro_system_service.Data
{
    public class EventsContext : DbContext // -> DbContext: representação do banco de dados
    {
        public DbSet<EventsModel> Events { get; set; }
        public DbSet<RegistrationModel> Registration { get; set; }
        public DbSet<ExtrafieldModel> ExtraField { get; set; }
        public DbSet<FieldanswerModel> FieldAnswers { get; set; }
        public EventsContext(DbContextOptions<EventsContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(connectionString: "Data Source=events.sqlite");
        }
        
    }

}
