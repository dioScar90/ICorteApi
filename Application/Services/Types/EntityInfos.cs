namespace ICorteApi.Application.Services;

public record EntityInfos(
    bool Exists = false,
    bool BelongsToMe = false
);