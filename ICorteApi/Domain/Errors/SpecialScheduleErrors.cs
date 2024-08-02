namespace ICorteApi.Domain.Errors;

public static class SpecialScheduleErrors
{
    public static readonly Error CreateError = new("Create Error", "Não foi possível criar o horário especial");
    public static readonly Error UpdateError = new("Update Error", "Não foi possível atualizar o horário especial");
    public static readonly Error RemoveError = new("Remove Error", "Não foi possível excluir o horário especial");
}
