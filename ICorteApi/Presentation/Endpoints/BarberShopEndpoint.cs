using ICorteApi.Application.Interfaces;
using ICorteApi.Application.Dtos;
using ICorteApi.Presentation.Extensions;
using ICorteApi.Presentation.Enums;
using FluentValidation;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Presentation.Endpoints;

public static class BarberShopEndpoint
{
    private static readonly string INDEX = "";
    private static readonly string ENDPOINT_PREFIX = EndpointPrefixes.BarberShop;
    private static readonly string ENDPOINT_NAME = EndpointNames.BarberShop;

    public static void Map(WebApplication app)
    {
        var group = app.MapGroup(ENDPOINT_PREFIX)
            .WithTags(ENDPOINT_NAME)
            .RequireAuthorization();

        group.MapGet(INDEX, GetAllBarberShops);
        group.MapGet("{id}", GetBarberShop);
        group.MapPost(INDEX, CreateBarberShop);
        group.MapPut("{id}", UpdateBarberShop);
        group.MapDelete("{id}", DeleteBarberShop);
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
        var response = await service.GetByIdAsync(id);

        if (!response.IsSuccess)
            return Results.NotFound();

        var barberShopDto = response.Value!.CreateDto();
        return Results.Ok(barberShopDto);
    }
    
    public static async Task<IResult> GetAllBarberShops(
        int page,
        int pageSize,
        IBarberShopService service,
        IBarberShopErrors errors)
    {
        var response = await service.GetAllAsync(page, pageSize);

        if (!response.IsSuccess)
            errors.ThrowNotFoundException();
        
        var dtos = response.Values!
            .Select(b => b.CreateDto())
            .ToArray();

        return Results.Ok(dtos);
    }

    public static async Task<IResult> CreateBarberShop(
        BarberShopDtoRequest dto,
        IValidator<BarberShopDtoRequest> validator,
        IBarberShopService service,
        IUserService userService,
        IBarberShopErrors errors)
    {
        var respUser = await userService.GetMeAsync();

        if (!respUser.IsSuccess)
            return Results.Unauthorized();

        var user = respUser.Value!;
        
        var validationResult = validator.Validate(dto);
        
        if (!validationResult.IsValid)
            errors.ThrowValidationException(validationResult.ToDictionary());

        var response = await service.CreateAsync(user.Id, dto);

        if (!response.IsSuccess)
            errors.ThrowCreateException();

        return GetCreatedResult(response.Value!.Id);
    }

    public static async Task<IResult> UpdateBarberShop(
        int id,
        BarberShopDtoRequest dto,
        IValidator<BarberShopDtoRequest> validator,
        IBarberShopService service,
        IBarberShopErrors errors)
    {
        var validationResult = validator.Validate(dto);
        
        if (!validationResult.IsValid)
            errors.ThrowValidationException(validationResult.ToDictionary());
        
        var response = await service.UpdateAsync(dto, id);

        if (!response.IsSuccess)
            errors.ThrowUpdateException();

        return Results.NoContent();
    }

    public static async Task<IResult> DeleteBarberShop(
        int id,
        IBarberShopService service,
        IBarberShopErrors errors)
    {
        var response = await service.DeleteAsync(id);

        if (!response.IsSuccess)
            errors.ThrowDeleteException();

        return Results.NoContent();
    }
}
