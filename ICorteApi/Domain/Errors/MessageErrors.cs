namespace ICorteApi.Domain.Errors;

public static class MessageErrors
{
    public static readonly Error CreateError = new("Create Error", "Não foi possível criar a mensagem");
    public static readonly Error UpdateError = new("Update Error", "Não foi possível atualizar a mensagem");
    public static readonly Error RemoveError = new("Remove Error", "Não foi possível excluir a mensagem");
}
