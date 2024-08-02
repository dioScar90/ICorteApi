namespace ICorteApi.Domain.Errors;

public static class ServiceErrors
{
    public static readonly Error CreateError = new("Create Error", "Não foi possível criar o serviço");
    public static readonly Error UpdateError = new("Update Error", "Não foi possível atualizar o serviço");
    public static readonly Error RemoveError = new("Remove Error", "Não foi possível excluir o serviço");
}
