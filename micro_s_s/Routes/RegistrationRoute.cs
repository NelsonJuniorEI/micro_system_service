using Microsoft.EntityFrameworkCore;
using micro_s_s.Models;
using micro_system_service.Data;
using Event.Models;

namespace micro_system_service.Routes
{
    public static class RegistrationRoute
    {
        public static async Task RegistrationRoutes(this WebApplication app)
        {
            var route = app.MapGroup("registration");

            var unused = route.MapPost(
                "/",
                static async (RegistrationRequest req, EventsContext context) =>
                {
                    var eventExists = await context.Events.AnyAsync(e => e.Id == req.Id_Event);
                    if (!eventExists)
                        return Results.BadRequest("Evento não encontrado.");

                    var registration = new RegistrationModel(
                        req.Registration_Name,
                        req.Registration_Email,
                        req.WhatsApp,
                        req.Id_Event
                    );

                    await context.AddAsync(registration);
                    await context.SaveChangesAsync();

                    foreach (var field in req.Fields)
                    {
                        if (field == null || field.Id == null || string.IsNullOrWhiteSpace(field.Value))
                            return Results.BadRequest("Todos os campos adicionais devem conter ID e valor.");

                        var fieldExists = await context.ExtraField.AnyAsync(f => f.Id == field.Id);
                        if (!fieldExists)
                            return Results.BadRequest($"Campo extra com ID {field.Id} não encontrado.");

                        var answer = new FieldanswerModel(field.Id, registration.Id, field.Value);
                        await context.AddAsync(answer);
                        await context.SaveChangesAsync();
                    }

                    return Results.Created($"/registration/{registration.Id}", registration);
                }
            );

            route.MapGet(
                "/",
                async (EventsContext context) =>
                {
                    var e = await context.Registration.ToListAsync();
                    return Results.Ok(e);
                }
            );

            route.MapGet(
                "/{id:Guid}",
                async (Guid id, EventsContext context) =>
                {
                    var e = await context.Registration.Where(x => x.Id_Event == id).ToListAsync();
                    if (e == null)
                        return Results.NotFound("Nenhum usuário registrado nesse evento.");

                    return Results.Ok(e);
                }
            );

            route.MapPut(
                "{id:Guid}",
                async (Guid id, RegistrationRequest req, EventsContext context) =>
                {
                    var e = await context.Registration.FirstOrDefaultAsync(x => x.Id == id);
                    if (e == null)
                        return Results.NotFound();
                    e.ChangeR_Name(req.Registration_Name);
                    e.ChangeEmail(req.Registration_Email);
                    e.ChangeWhatsApp(req.WhatsApp);
                    await context.SaveChangesAsync();
                    return Results.Ok(e);
                }
            );

            route.MapDelete(
                "{id:Guid}",
                async (Guid id, EventsContext context) =>
                {
                    var e = await context.Registration.FirstOrDefaultAsync(x => x.Id == id);
                    if (e == null)
                        return Results.NotFound();

                    e.SetInactive();
                    await context.SaveChangesAsync();
                    return Results.Ok(e);
                }
            );
        }
    }
}
