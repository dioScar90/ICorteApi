namespace ICorteApi.Domain.Errors;

public static class ReportErrors
{
    public static readonly Error CreateError = new("Create Error", "Não foi possível criar a avaliação");
    public static readonly Error UpdateError = new("Update Error", "Não foi possível atualizar a avaliação");
    public static readonly Error RemoveError = new("Remove Error", "Não foi possível excluir a avaliação");
}
