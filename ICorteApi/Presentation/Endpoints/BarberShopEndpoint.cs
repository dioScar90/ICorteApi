using ICorteApi.Application.Interfaces;
using ICorteApi.Application.Dtos;
using ICorteApi.Presentation.Extensions;
using ICorteApi.Presentation.Enums;
using FluentValidation;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Domain.Enums;

namespace ICorteApi.Presentation.Endpoints;

public static class BarberShopEndpoint
{
    private static readonly string INDEX = "";
    private static readonly string ENDPOINT_PREFIX = EndpointPrefixes.BarberShop;
    private static readonly string ENDPOINT_NAME = EndpointNames.BarberShop;

    public static IEndpointRouteBuilder MapBarberShopEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ENDPOINT_PREFIX)
            .WithTags(ENDPOINT_NAME);
        
        group.MapPost(INDEX, CreateBarberShop)
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapGet("{id}", GetBarberShop)
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));
        
        group.MapPut("{id}", UpdateBarberShop)
            .RequireAuthorization(nameof(PolicyUserRole.BarberShopOrHigh));
        
        group.MapDelete("{id}", DeleteBarberShop)
            .RequireAuthorization(nameof(PolicyUserRole.BarberShopOrHigh));
        
        return app;
    }
    
    public static IResult GetCreatedResult(int newId)
    {
        string uri = EndpointPrefixes.BarberShop + "/" + newId;
        object value = new { Message = "Barbearia criada com sucesso" };
        return Results.Created(uri, value);
    }

    public static async Task<IResult> GetBarberShop(
        int id,
        IBarberShopService service,
        IBarberShopErrors errors)
    {
        var resp = await service.GetByIdAsync(id);

        if (!resp.IsSuccess)
            errors.ThrowNotFoundException(resp.Error);

        var barberShopDto = resp.Value!.CreateDto();
        return Results.Ok(barberShopDto);
    }
    
    public static async Task<IResult> CreateBarberShop(
        BarberShopDtoRequest dto,
        IValidator<BarberShopDtoRequest> validator,
        IBarberShopService service,
        IUserService userService,
        IBarberShopErrors errors)
    {
        dto.CheckAndThrowExceptionIfInvalid(validator, errors);

        int ownerId = userService.GetMyUserId();
        var resp = await service.CreateAsync(dto, ownerId);

        if (!resp.IsSuccess)
            errors.ThrowCreateException(resp.Error);
        
        return GetCreatedResult(resp.Value!.Id);
    }

    public static async Task<IResult> UpdateBarberShop(
        int id,
        BarberShopDtoRequest dto,
        IValidator<BarberShopDtoRequest> validator,
        IBarberShopService service,
        IUserService userService,
        IBarberShopErrors errors)
    {
        dto.CheckAndThrowExceptionIfInvalid(validator, errors);
        
        int ownerId = userService.GetMyUserId();
        var resp = await service.UpdateAsync(dto, id, ownerId);

        if (!resp.IsSuccess)
            errors.ThrowUpdateException(resp.Error);

        return Results.NoContent();
    }

    public static async Task<IResult> DeleteBarberShop(
        int id,
        IBarberShopService service,
        IUserService userService,
        IBarberShopErrors errors)
    {
        int ownerId = userService.GetMyUserId();
        var resp = await service.DeleteAsync(id, ownerId);

        if (!resp.IsSuccess)
            errors.ThrowDeleteException(resp.Error);
        
        return Results.NoContent();
    }
}
