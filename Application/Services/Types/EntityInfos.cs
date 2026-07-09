namespace ICorteApi.Application.Services;

public record EntityInfos(
    bool Exists = false,
    bool BelongsToCurrentUser = false,
    bool BelongsToBarberShop = true,
    bool BelongsToDay = true
);