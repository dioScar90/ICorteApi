namespace ICorteApi.Domain.Errors;

public static class AppointmentErrors
{
    public static readonly Error CreateError = new("Create Error", "Não foi possível criar o agendamento");
    public static readonly Error UpdateError = new("Update Error", "Não foi possível atualizar o agendamento");
    public static readonly Error RemoveError = new("Remove Error", "Não foi possível excluir o agendamento");
}
