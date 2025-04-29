using micro_s_s.Models;
using micro_system_service.Data;
using Microsoft.EntityFrameworkCore;

namespace micro_s_s.Routes
{
    public static class FieldAnswersRoute
    {
        public static void FieldanswerRoutes(this WebApplication app)
        {
            var route = app.MapGroup("fieldanswer");

            route.MapPost(
                "/",
                async (FieldanswerRequest req, EventsContext context) =>
                {

                    var fieldExists = await context.ExtraField.AnyAsync(f => f.Id == req.Id_fields);
                    var regExists = await context.Registration.AnyAsync(r => r.Id == req.Id_Registered);

                    if (!fieldExists)
                        return Results.BadRequest("Field not found.");

                    var e = new FieldanswerModel(req.Id_fields, req.Id_Registered, req.Answer);

                    await context.AddAsync(e);
                    await context.SaveChangesAsync();
                    return Results.Created($"/fieldanswer/{e.Id}", e);
                }
            );

            route.MapGet(
                "/",
                async (EventsContext context) =>
                {
                    var e = await context.FieldAnswers.ToListAsync();
                    return Results.Ok(e);
                }
            );

            route.MapGet(
                "{id:Guid}",
                async (Guid id, EventsContext context) =>
                {
                    var fields = await context
                        .ExtraField.Where(x => x.Id_Event == id).ToListAsync();

                    var fieldanswer = new List<FieldAnswerDto>();

                    foreach (var field in fields)
                    {
                        var e = await context.FieldAnswers.Where(x => x.Id_Fields == field.Id).ToListAsync();
                        if (e == null) continue;

                        foreach (var item in e)
                        {
                            fieldanswer.Add(new FieldAnswerDto
                            {
                                registration_id = item.Id_Registered,
                                field_id = item.Id_Fields,
                                field_name = field.OtherFields,
                                answer = item.Answer
                            });
                        }
                    }

                    return Results.Ok(fieldanswer);
                }
            );

            route.MapPut(
                "{id:Guid}",
                async (Guid id, FieldanswerRequest req, EventsContext context) =>
                {
                    var e = await context.FieldAnswers.FirstOrDefaultAsync(x => x.Id == id);
                    if (e == null)
                        return Results.NotFound();

                    e.ChangeAnswer(req.Answer);

                    await context.SaveChangesAsync();
                    return Results.Ok(e);
                }
            );
        }
    }
}

public class FieldAnswerDto
{
    public Guid registration_id { get; set; }
    public Guid field_id { get; set; }
    public string field_name { get; set; }
    public string answer { get; set; }
}
