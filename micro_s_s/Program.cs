using micro_s_s.Routes;
using micro_system_service.Data;
using micro_system_service.Routes;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adicionando serviços ao container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<EventsContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Configurando o CORS para permitir solicitações de qualquer origem
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()  // Permite qualquer origem
              .AllowAnyHeader()  // Permite qualquer cabeçalho
              .AllowAnyMethod(); // Permite qualquer método (GET, POST, etc.)
    });
});

var app = builder.Build();

// Habilitando o Swagger e Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Aplicar a política CORS
app.UseCors("AllowAll");

// Registrar as rotas do seu serviço
app.EventRoutes();
app.RegistrationRoutes();
app.ExtrafieldRoutes();
app.FieldanswerRoutes();

// Rodar a aplicação
app.Run();
