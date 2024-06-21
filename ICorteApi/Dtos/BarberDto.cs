namespace ICorteApi.Dtos;

public record BarberDtoResponse(
    int Id,
    string Name,
    AddressDtoResponse Address
);

public record BarberDtoRequest(
    int? Id,
    string Name,
    AddressDtoRequest Address
);
