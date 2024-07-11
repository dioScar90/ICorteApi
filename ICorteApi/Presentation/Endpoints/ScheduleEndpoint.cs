// using Microsoft.EntityFrameworkCore;
// using ICorteApi.Infraestructure.Context;
// using ICorteApi.Application.Dtos;
// using ICorteApi.Presentation.Extensions;
// using ICorteApi.Domain.Entities;

// namespace ICorteApi.Presentation.Endpoints;

// public static class ScheduleEndpoint
// {
//     private static readonly string INDEX = "";
//     private static readonly string ENDPOINT_PREFIX = "schedule";
//     private static readonly string ENDPOINT_NAME = "Schedule";

//     public static void Map(WebApplication app)
//     {
//         var group = app.MapGroup(ENDPOINT_PREFIX)
//             .WithTags(ENDPOINT_NAME)
//             .RequireAuthorization();

//         // group.MapGet(INDEX, GetAllSchedules);
//         group.MapGet("{id}", GetSchedule);
//         group.MapGet("{barberId}/availability", GetAvailableTimeSlots);
//         group.MapPost(INDEX, CreateSchedule);
//         group.MapPut("{id}", UpdateSchedule);
//         group.MapDelete("{id}", DeleteSchedule);
//     }

//     public static async Task<IResult> GetSchedule(int id, AppDbContext context)
//     {
//         var schedule = await context.Schedules
//             .SingleOrDefaultAsync(b => b.Id == id);

//         if (schedule is null)
//             return Results.NotFound();

//         var scheduleDto = schedule.CreateDto<ScheduleDtoResponse>();
//         return Results.Ok(scheduleDto);
//     }

//     // public static async Task<IResult> GetAllSchedules(int page, int perPage, AppDbContext context)
//     // {
//     //     var schedules = new ScheduleRepository(context)
//     //         .GetAll(page, perPage);

//     //     if (!schedules.Any())
//     //         Results.NotFound();

//     //     var dtos = await schedules
//     //         .Select(b => ScheduleToDtoResponse(b))
//     //         .ToListAsync();

//     //     return Results.Ok(dtos);
//     // }

//     public static async Task<IResult> GetAvailableTimeSlots(int barberId, DateTime date)
//     {
//         var availableTimeSlots = _scheduleService.GetAvailableTimeSlots(barberId, date);
//         return Results.Ok(availableTimeSlots);
//     }

//     public static async Task<IResult> GetAvailableTimeSlots(int barberId, DateTime date, AppDbContext context)
//     {
//         var recurringSchedules = context.RecurringSchedules
//             .Where(rs => rs.BarberId == barberId && rs.DayOfWeek == date.DayOfWeek)
//             .ToList();

//         var appointments = context.Appointments
//             .Where(a => a.BarberId == barberId && a.AppointmentDate.Date == date.Date)
//             .ToList();

//         var availableTimeSlots = new List<TimeSlotDtoResponse>();

//         foreach (var schedule in recurringSchedules)
//         {
//             var startTime = schedule.StartTime;
//             while (startTime < schedule.EndTime)
//             {
//                 var endTime = startTime.Add(TimeSpan.FromMinutes(30)); // Assume 30 minutes per appointment

//                 if (!appointments.Any(a => a.StartTime < endTime && a.EndTime > startTime))
//                 {
//                     availableTimeSlots.Add(new(startTime, endTime));
//                 }

//                 startTime = endTime;
//             }
//         }

//         return availableTimeSlots;
//     }

//     public static async Task<IResult> CreateSchedule(ScheduleDtoRequest dto, AppDbContext context)
//     {
//         try
//         {
//             var newSchedule = dto.CreateEntity<Schedule>();

//             await context.Schedules.AddAsync(newSchedule!);
//             await context.SaveChangesAsync();

//             return Results.Created($"/{ENDPOINT_PREFIX}/{newSchedule.Id}", new { Message = "Agendamento criado com sucesso" });
//         }
//         catch (Exception ex)
//         {
//             return Results.BadRequest(ex.Message);
//         }
//     }

//     public static async Task<IResult> UpdateSchedule(int id, ScheduleDtoRequest dto, AppDbContext context)
//     {
//         try
//         {
//             var schedule = await context.Schedules.SingleOrDefaultAsync(b => b.Id == id);

//             if (schedule is null)
//                 return Results.NotFound();

//             schedule.DayOfWeek = dto.DayOfWeek;
//             schedule.StartTime = dto.StartTime;
//             schedule.EndTime = dto.EndTime;
//             schedule.IsAvailable = dto.IsAvailable;

//             schedule.UpdatedAt = DateTime.UtcNow;

//             await context.SaveChangesAsync();
//             return Results.Ok(new { Message = "Agendamento atualizado com sucesso" });
//         }
//         catch (Exception ex)
//         {
//             return Results.BadRequest(ex.Message);
//         }
//     }

//     public static async Task<IResult> DeleteSchedule(int id, AppDbContext context)
//     {
//         try
//         {
//             var schedule = await context.Schedules.SingleOrDefaultAsync(b => b.Id == id);

//             if (schedule is null)
//                 return Results.NotFound();

//             schedule.UpdatedAt = DateTime.UtcNow;
//             schedule.IsDeleted = true;

//             await context.SaveChangesAsync();
//             return Results.NoContent();
//         }
//         catch (Exception ex)
//         {
//             return Results.BadRequest(ex.Message);
//         }
//     }
// }
