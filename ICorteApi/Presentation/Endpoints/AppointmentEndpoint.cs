// using Microsoft.EntityFrameworkCore;
// using ICorteApi.Infraestructure.Context;
// using ICorteApi.Presentation.Extensions;
// using ICorteApi.Application.Dtos;
// using ICorteApi.Domain.Entities;

// namespace ICorteApi.Presentation.Endpoints;

// public static class AppointmentEndpoint
// {
//     private const string INDEX = "";
//     private const string ENDPOINT_PREFIX = "appointment";
//     private const string ENDPOINT_NAME = "Appointment";

//     public static void Map(WebApplication app)
//     {
//         var group = app.MapGroup(ENDPOINT_PREFIX)
//             .WithName(ENDPOINT_NAME)
//             .RequireAuthorization();

//         // group.MapGet(INDEX, GetAppointmentes);
//         group.MapGet("{id}", GetAppointment);
//         group.MapPost(INDEX, CreateAppointment);
//         group.MapPut("{id}", UpdateAppointment);
//         group.MapDelete("{id}", DeleteAppointment);
//     }

//     public static async Task<IResult> GetAppointment(int id, AppDbContext context)
//     {
//         var appointment = await context.Appointments
//             .SingleOrDefaultAsync(a => a.Id == id);

//         if (appointment is null)
//             return Results.NotFound("Agendamento não encontrado");

//         var appointmentDto = appointment.CreateDto<AppointmentDtoResponse>();
//         return Results.Ok(appointmentDto);
//     }

//     public static async Task<IResult> CreateAppointment(AppointmentDtoRequest dto, AppDbContext context)
//     {
//         try
//         {
//             var newAppointment = dto.CreateEntity<Appointment>()!;

//             await context.Appointments.AddAsync(newAppointment);
//             await context.SaveChangesAsync();

//             return Results.Created($"/{ENDPOINT_PREFIX}/{newAppointment.Id}", new { Message = "Agendamento criado com sucesso" });
//         }
//         catch (Exception ex)
//         {
//             return Results.BadRequest(ex.Message);
//         }
//     }

//     public static async Task<IResult> UpdateAppointment(int id, AppointmentDtoRequest dto, AppDbContext context)
//     {
//         try
//         {
//             var appointment = await context.Appointments.SingleOrDefaultAsync(a => a.Id == id);

//             if (appointment is null)
//                 return Results.NotFound();

//             appointment.AppointmentDate = dto.AppointmentDate;
//             appointment.UpdatedAt = DateTime.UtcNow;

//             await context.SaveChangesAsync();
//             return Results.Ok(new { Message = "Agendamento atualizado com sucesso" });
//         }
//         catch (Exception ex)
//         {
//             return Results.BadRequest(ex.Message);
//         }
//     }

//     public static async Task<IResult> DeleteAppointment(int id, AppDbContext context)
//     {
//         try
//         {
//             var appointment = await context.Appointments.SingleOrDefaultAsync(a => a.Id == id);

//             if (appointment is null)
//                 return Results.NotFound();

//             appointment.UpdatedAt = DateTime.UtcNow;
//             appointment.IsDeleted = true;

//             await context.SaveChangesAsync();
//             return Results.NoContent();
//         }
//         catch (Exception ex)
//         {
//             return Results.BadRequest(ex.Message);
//         }
//     }
// }
