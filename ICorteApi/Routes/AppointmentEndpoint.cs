using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ICorteApi.Entities;
using ICorteApi.Context;
using ICorteApi.Dtos;
using ICorteApi.Extensions;

namespace ICorteApi.Routes;

public static class AppointmentEndpoint
{
    public static void MapAppointmentEndpoint(this IEndpointRouteBuilder app)
    {
        const string INDEX = "";
        var group = app.MapGroup("appointment");

        // group.MapGet(INDEX, GetAppointmentes);
        group.MapGet("{id}", GetAppointment);
        group.MapPost(INDEX, CreateAppointment);
        group.MapPut("{id}", UpdateAppointment);
        group.MapDelete("{id}", DeleteAppointment);
    }
    
    public static async Task<IResult> GetAppointment(int id, AppDbContext context)
    {
        var appointment = await context.Appointments
            .SingleOrDefaultAsync(a => a.Id == id);

        if (appointment is null)
            return Results.NotFound("Agendamento não encontrado");

        var appointmentDto = appointment.CreateDto<AppointmentDtoResponse>();
        return Results.Ok(appointmentDto);
    }
    
    public static async Task<IResult> CreateAppointment(AppointmentDtoRequest dto, AppDbContext context)
    {
        try
        {
            var newAppointment = dto.CreateEntity<Appointment>()!;
            
            await context.Appointments.AddAsync(newAppointment);
            await context.SaveChangesAsync();
            
            return Results.Created($"/appointment/{newAppointment.Id}", new { Message = "Agendamento criado com sucesso" });
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    public static async Task<IResult> UpdateAppointment(int id, AppointmentDtoRequest dto, AppDbContext context)
    {
        try
        {
            var appointment = await context.Appointments.SingleOrDefaultAsync(a => a.Id == id);

            if (appointment is null)
                return Results.NotFound();

            appointment.AppointmentDate = dto.AppointmentDate;
            appointment.UpdatedAt = DateTime.UtcNow;
            
            await context.SaveChangesAsync();
            return Results.Ok(new { Message = "Agendamento atualizado com sucesso" });
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    public static async Task<IResult> DeleteAppointment(int id, AppDbContext context)
    {
        try
        {
            var appointment = await context.Appointments.SingleOrDefaultAsync(a => a.Id == id);

            if (appointment is null)
                return Results.NotFound();
            
            appointment.UpdatedAt = DateTime.UtcNow;
            appointment.IsDeleted = true;

            await context.SaveChangesAsync();
            return Results.NoContent();
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }
}
