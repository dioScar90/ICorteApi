namespace ICorteApi.Domain.Errors;

public static class UserErrors
{
    public static readonly Error CreateError = new("Create Error", "Não foi possível criar o usuário");
    public static readonly Error UpdateError = new("Update Error", "Não foi possível atualizar o usuário");
    public static readonly Error RemoveError = new("Remove Error", "Não foi possível excluir o usuário");
}
