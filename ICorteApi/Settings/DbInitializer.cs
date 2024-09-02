using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Enums;
using ICorteApi.Infraestructure.Context;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Settings;

public static class DbInitializer
{
    public static async void Initialize(AppDbContext context)
    {
        // Garante que o banco de dados está criado
        await context.Database.EnsureCreatedAsync();

        // Verifica se já existem dados no banco de dados
        if (await context.Users.AnyAsync())
        {
            return;   // Banco de dados foi inicializado previamente
        }

        // Adicione os dados iniciais aqui
        var users = GetSomeUsers();

        foreach (User u in users)
        {
            context.Users.Add(u);
        }
        
        // Salva as mudanças
        await context.SaveChangesAsync();
    }

    private static User[] GetSomeUsers() =>
        [
            new(new("diogols@live.com", "diogols@live.com")),
            new(new("admin@example.com", "admin@example.com")),
            new(new("user@example.com", "user@example.com")),
        ];

    private static Profile[] GetSomeProfiles() =>
        [
            new(new("Diogo", "Scarmagnani", Gender.Male, "19912354698")),
            new(new("Lionel", "Messi", Gender.Male, "11912345678")),
            new(new("Pato", "Abbondanzieri", Gender.Male, "19988145678")),
        ];

    private static BarberShop[] GetSomeBarberShops() =>
        [
            new(new("Barbearia de Messi", "Que, bobo!?", "18981234567", "lionel@messi.com.br")),
            new(new("Barbearia de Abbondanzieri", "Que, bobo!?", "19988145678", "pato@abbondanzieri.com")),
        ];

    private static Address[] GetSomeAddresses() =>
        [
            new(new("R Jose Santo de Dios", "10", null, "Centro", "Barcelona", State.SP, "19167000", "Brasil")),
            new(new("R Jose Santo de Dios", "10", null, "Centro", "Barcelona", State.SP, "19167000", "Brasil")),
        ];

    // private static Appointment[] GetSomeAppointments() =>
    //     [
    //         new(new("diogols@live.com", "diogols@live.com")),
    //         new(new("admin@example.com", "admin@example.com")),
    //         new(new("user@example.com", "user@example.com")),
    //     ];

    // private static Message[] GetSomeMessages() =>
    //     [
    //         new(new("diogols@live.com", "diogols@live.com")),
    //         new(new("admin@example.com", "admin@example.com")),
    //         new(new("user@example.com", "user@example.com")),
    //     ];

    // private static Payment[] GetSomePayments() =>
    //     [
    //         new(new("diogols@live.com", "diogols@live.com")),
    //         new(new("admin@example.com", "admin@example.com")),
    //         new(new("user@example.com", "user@example.com")),
    //     ];

    private static RecurringSchedule[] GetSomeRecurringSchedules() =>
        [
            new(new(
                DayOfWeek.Monday,
                new(09, 00, 00),
                new(18, 00, 00)
            )),
            new(new(
                DayOfWeek.Tuesday,
                new(09, 00, 00),
                new(18, 00, 00)
            )),
            new(new(
                DayOfWeek.Wednesday,
                new(09, 00, 00),
                new(18, 00, 00)
            )),
            new(new(
                DayOfWeek.Thursday,
                new(09, 00, 00),
                new(18, 00, 00)
            )),
            new(new(
                DayOfWeek.Friday,
                new(09, 00, 00),
                new(18, 00, 00)
            )),
            new(new(
                DayOfWeek.Saturday,
                new(09, 00, 00),
                new(12, 00, 00)
            )),

            new(new(
                DayOfWeek.Monday,
                new(09, 00, 00),
                new(18, 00, 00)
            )),
            new(new(
                DayOfWeek.Tuesday,
                new(09, 00, 00),
                new(18, 00, 00)
            )),
            new(new(
                DayOfWeek.Wednesday,
                new(09, 00, 00),
                new(18, 00, 00)
            )),
            new(new(
                DayOfWeek.Thursday,
                new(09, 00, 00),
                new(18, 00, 00)
            )),
            new(new(
                DayOfWeek.Friday,
                new(09, 00, 00),
                new(18, 00, 00)
            )),
        ];

    private static SpecialSchedule[] GetSomeSpecialSchedules() =>
        [
            new(new(
                new(2024, 09, 02),
                new(08, 00, 00),
                new(12, 00, 00)
            )),
            new(new(
                new(2024, 09, 07),
                "Feriado"
            )),
        ];

    // private static Report[] GetSomeReports() =>
    //     [
    //         new(new("diogols@live.com", "diogols@live.com")),
    //         new(new("admin@example.com", "admin@example.com")),
    //         new(new("user@example.com", "user@example.com")),
    //     ];

    private static Service[] GetSomeServices() =>
        [
            new(new(
                "Corte",
                "Corte do bom",
                25M,
                new(00, 30, 00)
            )),
            new(new(
                "Barba",
                "A sua barba muito bem cuidada",
                15M,
                new(00, 15, 00)
            )),
            new(new(
                "Barba e Cabelo",
                "Você todo estiloso",
                38M,
                new(00, 45, 00)
            )),
            new(new(
                "Conversa",
                "Bora jogar conversa fora",
                99.9M,
                new(01, 00, 00)
            )),
        ];
}

