using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Base;

namespace ICorteApi.Domain.Entities;

public sealed class BarberShop : BasePrimaryKeyEntity<BarberShop, int>
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public string ComercialNumber { get; private set; }
    public string ComercialEmail { get; private set; }
    public string? ImageUrl { get; private set; }

    public int OwnerId { get; init; }
    public User Owner { get; init; }

    public Address Address { get; init; }
    public ICollection<RecurringSchedule> RecurringSchedules { get; init; }
    public ICollection<SpecialSchedule> SpecialSchedules { get; init; }
    public ICollection<Service> Services { get; init; }
    public ICollection<Report> Reports { get; init; }

    private BarberShop() {}

    public BarberShop(BarberShopDtoRequest dto, int? ownerId = null)
    {
        Name = dto.Name;
        Description = dto.Description ?? default;
        ComercialNumber = dto.ComercialNumber;
        ComercialEmail = dto.ComercialEmail;

        if (dto.Address is AddressDtoRequest addressDto)
            Address = new(addressDto);

        if (dto.RecurringSchedules is RecurringScheduleDtoRequest[] rsDto)
            RecurringSchedules = rsDto.Select(rs => new RecurringSchedule(rs)).ToList();

        if (dto.SpecialSchedules is SpecialScheduleDtoRequest[] ssDto)
            SpecialSchedules = ssDto.Select(ss => new SpecialSchedule(ss)).ToList();

        if (dto.Services is ServiceDtoRequest[] serviceDto)
            Services = serviceDto.Select(s => new Service(s)).ToList();

        if (dto.Reports is ReportDtoRequest[] reportDto)
            Reports = reportDto.Select(r => new Report(r)).ToList();

        OwnerId = ownerId ?? default;
    }
    
    private void UpdateByBarberShopDto(BarberShopDtoRequest dto, DateTime? utcNow)
    {
        utcNow ??= DateTime.UtcNow;

        Name = dto.Name;
        Description = dto.Description ?? null;
        ComercialNumber = dto.ComercialNumber;
        ComercialEmail = dto.ComercialEmail;

        var itemsToUpdateByRecurringSchedules =
            from os in dto.RecurringSchedules
            join bs in RecurringSchedules on os.DayOfWeek equals bs.DayOfWeek
            select new { bs, os };

        foreach (var updatingItems in itemsToUpdateByRecurringSchedules)
            updatingItems.bs.UpdateEntityByDto(updatingItems.os, utcNow);

        if (dto.Address is not null)
            Address?.UpdateEntityByDto(dto.Address, utcNow);

        UpdatedAt = utcNow;
    }
    
    public override void UpdateEntityByDto(IDtoRequest<BarberShop> requestDto, DateTime? utcNow = null)
    {
        if (requestDto is BarberShopDtoRequest dto)
            UpdateByBarberShopDto(dto, utcNow);
            
        throw new Exception("Dados enviados inválidos");
    }
}
