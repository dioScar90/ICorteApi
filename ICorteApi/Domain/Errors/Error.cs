namespace ICorteApi.Domain.Errors;

public sealed record Error(
    string Code,
    string Description
)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new(string.Empty, string.Empty);
    public static readonly Error PersonNotFound = new("NotFound", "Não há ninguém com esse nome aqui");
    public static readonly Error UserNotFound = new("NotFound", "Usuário não encontrado");
    public static readonly Error BarberShopNotFound = new("NotFound", "Barbearia não encontrada");
    public static readonly Error BadSave = new("BadRequest", "Não foi possível concluir a operação, tente novamente");
}
