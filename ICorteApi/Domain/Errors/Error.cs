namespace ICorteApi.Domain.Errors;

public sealed record Error(string Code, string Description)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new(string.Empty, string.Empty);
    public static readonly Error CreateError = new("Server Error", "Não foi possível criar o item");
    public static readonly Error UpdateError = new("Server Error", "Não foi possível atualizar o item");
    public static readonly Error RemoveError = new("Server Error", "Não foi possível remover o item");
    public static readonly Error ProfileNotFound = new("Not Found", "Não há ninguém com esse nome aqui");
    public static readonly Error UserNotFound = new("Not Found", "Usuário não encontrado");
    public static readonly Error BarberShopNotFound = new("Not Found", "Barbearia não encontrada");
    public static readonly Error RecurringScheduleNotFound = new("Not Found", "Horário de funcionamento não encontrado");
    public static readonly Error SpecialScheduleNotFound = new("Not Found", "Horário de funcionamento não encontrado");
    public static readonly Error TEntityNotFound = new("Not Found", "Item não encontrado");
    public static readonly Error BadSave = new("Bad Request", "Não foi possível concluir a operação, tente novamente");
    public static readonly Error Unauthorized = new("Unauthorized", "Não autorizado");
}
