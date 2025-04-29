using micro_system_service.Data;
using Microsoft.EntityFrameworkCore;
using Event.Models;
using micro_system_service.Models;
using micro_s_s.Models;

namespace micro_system_service.Routes
{
    public static class EventRoute
    {
        public static void EventRoutes(this WebApplication app)
        {
            var route = app.MapGroup("event");

            route.MapPost(
                "/",
                async (EventRequest req, EventsContext context) =>
                {
                    var e = new EventsModel(
                        req.Name_event,
                        req.Type_event,
                        req.Value_event,
                        req.Location_event,
                        req.Date_event
                    );

                    await context.AddAsync(e);
                    await context.SaveChangesAsync();

                    if (req.extra_fields != null)
                    {
                        foreach (var field in req.extra_fields)
                            await context.AddAsync(new ExtrafieldModel(field.Value, e.Id));
                    }

                    await context.SaveChangesAsync();

                    return Results.Created($"/Events/{e.Id}", e);
                }
            );

            route.MapGet(
                "/",
                async (EventsContext context) =>
                {
                    var e = await context.Events.Where(e => e.Type_event != "desativado").ToListAsync();
                    return Results.Ok(e);
                }
            );

            route.MapGet(
                "{id:Guid}",
                async (Guid id, EventsContext context) =>
                {
                    var e = await context.Events.FirstOrDefaultAsync(x => x.Id == id);
                    if (e == null)
                        return Results.NotFound();

                    return Results.Ok(e);
                }
            );

            route.MapPut(
                "{id:Guid}",
                async (Guid id, EventRequest req, EventsContext context) => //Request = requisição , Context = Banco de dados (Tabela)
                {
                    var e = await context.Events.FirstOrDefaultAsync(x => x.Id == id); // Vá por elementos/ context: Interagindo com o DB/ FirstOrDefault: O primeiro que atender as condições/ condição: Vendo se o Id do evento é igual ao da route.MapPut

                    if (e == null) //se o evento selecionado for nulo
                    {
                        return Results.NotFound("Evento não encontrado.");
                    }

                    e.ChangeAllData(
                        req.Name_event,
                        req.Type_event,
                        req.Value_event,
                        req.Location_event,
                        req.Date_event
                    );

                    var allfields = await context
                        .ExtraField.Where(ef => ef.Id_Event == e.Id)
                        .ToListAsync();

                    if (allfields != null)
                    {
                        foreach (var especificfield in allfields)
                        {
                            var existeCamporequest = req.extra_fields.Find(ef => ef.Id == especificfield.Id);
                            if (existeCamporequest != null)
                            {
                                var existeCampo = await context.ExtraField.FirstOrDefaultAsync(ef =>
                                    ef.Id == existeCamporequest.Id);

                                if (existeCampo != null)
                                {
                                    existeCampo.ChangeOtherFields(existeCamporequest.Value);
                                }
                                else
                                {
                                    await context.AddAsync(
                                        new ExtrafieldModel(existeCamporequest.Value, e.Id));
                                }
                            }
                            else if (!especificfield.OtherFields.Contains("<---desativado-->"))
                            {
                                especificfield.ChangeOtherFields(
                                    $"<---desativado--> {especificfield.OtherFields}");
                            }
                        }
                    }

                    if (req.extra_fields != null)
                    {
                        foreach (var field in req.extra_fields)
                        {
                            var existeCamporequest = await context.ExtraField
                                .FirstOrDefaultAsync(ef => ef.Id == field.Id);

                            if (existeCamporequest != null)
                            {
                                existeCamporequest.ChangeOtherFields(field.Value);
                            }
                            else
                            {
                                await context.AddAsync(new ExtrafieldModel(field.Value, e.Id));
                            }
                        }

                    }
                    ;
                    await context.SaveChangesAsync();
                    return Results.Ok();
                }
            );

            route.MapDelete(
                "{id:Guid}",
                async (Guid id, EventsContext context) =>
                {
                    var e = await context.Events.FirstOrDefaultAsync(x => x.Id == id);
                    if (e == null)
                        return Results.NotFound();

                    e.ChangeTypeDelete();
                    await context.SaveChangesAsync();
                    return Results.Ok(e);
                }
            );
        }
    }
}
