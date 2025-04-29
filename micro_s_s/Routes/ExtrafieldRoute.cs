using micro_s_s.Models;
using micro_system_service.Data;
using Microsoft.EntityFrameworkCore;

namespace micro_s_s.Routes
{
    public static class ExtrafieldRoute
    {
        public static void ExtrafieldRoutes(this WebApplication app)
        {
            var route = app.MapGroup("extrafield");

            route.MapPost(
                "/",
                async (ExtrafieldRequest req, EventsContext context) =>
                {
                    var e = new ExtrafieldModel(req.OtherFields, req.Id_Event);
                    await context.AddAsync(e);
                    await context.SaveChangesAsync();
                    return Results.Created($"/extrafield/{e.Id}", e);
                }
            );

            route.MapGet(
                "/",
                async (EventsContext context) =>
                {
                    var e = await context.ExtraField.ToListAsync();
                    return Results.Ok(e);
                }
            );

            route.MapGet(
                "events/{EventId:Guid}",
                async (Guid EventId, EventsContext context) =>
                {
                    var e = await context.ExtraField.Where(x => x.Id_Event == EventId && !x.OtherFields.Contains("<---desativado-->"))
                    .ToListAsync();
                    if (e == null)
                    {
                        return Results.NotFound();
                    }
                    return Results.Ok(e);
                }
            );

            route.MapGet(
                "{id:Guid}",
                async (Guid id, EventsContext context) =>
                {
                    var e = await context.ExtraField.FirstOrDefaultAsync(x => x.Id == id);
                    if (e == null)
                        return Results.NotFound();

                    return Results.Ok(e);
                }
            );

            route.MapPut(
                "{id:Guid}",
                async (Guid id, ExtrafieldRequest req, EventsContext context) =>
                {
                    var e = await context.ExtraField.FirstOrDefaultAsync(x => x.Id == id);
                    if (e == null)
                        return Results.NotFound();

                    e.ChangeOtherFields(req.OtherFields);
                    await context.SaveChangesAsync();
                    return Results.Ok(e);
                }
            );

            route.MapDelete(
                "{id:Guid}",
                async (Guid id, EventsContext context) =>
                {
                    var e = await context.ExtraField.FirstOrDefaultAsync(x => x.Id == id);
                    if (e == null)
                        return Results.NotFound();

                    await context.SaveChangesAsync();
                    return Results.Ok(e);
                }
            );
        }
    }
}