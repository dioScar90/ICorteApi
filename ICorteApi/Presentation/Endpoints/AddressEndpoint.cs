using ICorteApi.Application.Dtos;
using ICorteApi.Presentation.Extensions;
using ICorteApi.Presentation.Enums;
using FluentValidation;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Presentation.Endpoints;

public static class AddressEndpoint
{
    private static readonly string INDEX = "";
    private static readonly string ENDPOINT_PREFIX = EndpointPrefixes.BarberShop + "/{barberShopId}/" + EndpointPrefixes.Address;
    private static readonly string ENDPOINT_NAME = EndpointNames.Address;
    
    public static void Map(WebApplication app)
    {
        var group = app.MapGroup(ENDPOINT_PREFIX)
            .WithTags(ENDPOINT_NAME)
            .RequireAuthorization();
        
        group.MapGet("{id}", GetAddress);
        group.MapPost(INDEX, CreateAddress);
        group.MapPut("{id}", UpdateAddress);
        group.MapDelete("{id}", DeleteAddress);
    }

    public static IResult GetCreatedResult(int newId, int barberShopId)
    {
        string uri = EndpointPrefixes.BarberShop + "/" + barberShopId + "/" + EndpointPrefixes.Address + "/" + newId;
        object value = new { Message = "Endereço criado com sucesso" };
        return Results.Created(uri, value);
    }
    
    public static async Task<IResult> GetAddress(
        int barberShopId,
        int id,
        IAddressService service,
        IAddressErrors errors)
    {
        var res = await service.GetByIdAsync(id);

        if (!res.IsSuccess)
            errors.ThrowNotFoundException();

        var address = res.Value!;

        if (address.BarberShopId != barberShopId)
            errors.ThrowBadRequestException();

        var addressDto = address.CreateDto();
        return Results.Ok(addressDto);
    }

    public static async Task<IResult> CreateAddress(
        int barberShopId,
        AddressDtoRequest dto,
        IValidator<AddressDtoRequest> validator,
        IAddressService service,
        IAddressErrors errors)
    {
        var validationResult = validator.Validate(dto);
        
        if (!validationResult.IsValid)
            errors.ThrowValidationException(validationResult.ToDictionary());
                    
        var response = await service.CreateAsync(dto with { BarberShopId = barberShopId });

        if (!response.IsSuccess)
            errors.ThrowCreateException();
            
        return GetCreatedResult(response.Value!.Id, barberShopId);
    }

    public static async Task<IResult> UpdateAddress(
        int barberShopId,
        int id,
        AddressDtoRequest dto,
        IValidator<AddressDtoRequest> validator,
        IAddressService service,
        IAddressErrors errors)
    {
        var validationResult = validator.Validate(dto);
        
        if (!validationResult.IsValid)
            errors.ThrowValidationException(validationResult.ToDictionary());
        
        var response = await service.UpdateAsync(dto, id);

        if (!response.IsSuccess)
            errors.ThrowUpdateException();

        return Results.NoContent();
    }

    public static async Task<IResult> DeleteAddress(
        int barberShopId,
        int id,
        IAddressService service,
        IAddressErrors errors)
    {
        var response = await service.DeleteAsync(id);

        if (!response.IsSuccess)
            errors.ThrowDeleteException();
            
        return Results.NoContent();
    }
}
