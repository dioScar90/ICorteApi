namespace ICorteApi.Domain.Errors;

public static class BarberShopErrors
{
    public static readonly Error CreateError = new("Create Error", "Não foi possível criar a barbearia");
    public static readonly Error UpdateError = new("Update Error", "Não foi possível atualizar a barbearia");
    public static readonly Error RemoveError = new("Remove Error", "Não foi possível excluir a barbearia");
}
