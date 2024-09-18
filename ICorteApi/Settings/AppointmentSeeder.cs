// using Microsoft.AspNetCore.Identity;

// namespace ICorteApi.Settings;

// public static class AppointmentSeeder
// {
//     public static async Task SeedAppointments(IServiceProvider serviceProvider)
//     {
//         using var scope = serviceProvider.CreateScope();
//         var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

//         var appointments = new List<Appointment>
//         {
//             new(
//                 new(
//                     new DateOnly(2024, 9, 18),
//                     new TimeOnly(10, 00, 00),
//                     "Cliente quer manter o estilo anterior.",
//                     PaymentType.Card,
//                     []
//                     // 1, // João Silva
//                     // 1, // Barbearia do Messi
//                     // Services = new List<Service>
//                     // {
//                     //     new Service { Name = "Corte Messi", Duration = new TimeSpan(0, 40, 0), Price = 50M }
//                     // },
//                 ),
//                 [],
//                 1 // João Silva
//             ),
//             new Appointment
//             {
//                 Date = new DateTime(2024, 9, 18),
//                 StartTime = new TimeSpan(10, 00, 00),
//                 ClientId = 1, // João Silva
//                 BarberShopId = 1, // Barbearia do Messi
//                 Services = new List<Service>
//                 {
//                     new Service { Name = "Corte Messi", Duration = new TimeSpan(0, 40, 0), Price = 50M }
//                 },
//                 Notes = "Cliente quer manter o estilo anterior."
//             },
//             new Appointment
//             {
//                 Date = new DateTime(2024, 9, 19),
//                 StartTime = new TimeSpan(11, 30, 00),
//                 ClientId = 2, // Maria Oliveira
//                 BarberShopId = 2, // Barbearia do CR7
//                 Services = new List<Service>
//                 {
//                     new Service { Name = "Corte CR7", Duration = new TimeSpan(0, 45, 0), Price = 60M }
//                 },
//                 Notes = "Primeira vez do cliente na barbearia."
//             },
//             new Appointment
//             {
//                 Date = new DateTime(2024, 9, 19),
//                 StartTime = new TimeSpan(09, 00, 00),
//                 ClientId = 3, // Carlos Souza
//                 BarberShopId = 3, // Barbearia do Neymar
//                 Services = new List<Service>
//                 {
//                     new Service { Name = "Corte Neymar", Duration = new TimeSpan(0, 50, 0), Price = 70M }
//                 },
//                 Notes = "Cliente pediu estilo moderno."
//             },
//             new Appointment
//             {
//                 Date = new DateTime(2024, 9, 20),
//                 StartTime = new TimeSpan(14, 30, 00),
//                 ClientId = 4, // Ana Santos
//                 BarberShopId = 4, // Barbearia da Rosamaria
//                 Services = new List<Service>
//                 {
//                     new Service { Name = "Corte Rosa", Duration = new TimeSpan(0, 35, 0), Price = 55M }
//                 },
//                 Notes = "Cliente solicitou mudança completa no visual."
//             },
//             new Appointment
//             {
//                 Date = new DateTime(2024, 9, 21),
//                 StartTime = new TimeSpan(10, 00, 00),
//                 ClientId = 5, // Paulo Almeida
//                 BarberShopId = 5, // Barbearia da Alex
//                 Services = new List<Service>
//                 {
//                     new Service { Name = "Corte Alex", Duration = new TimeSpan(0, 45, 0), Price = 70M }
//                 },
//                 Notes = "Cliente quer corte discreto."
//             },
//             new Appointment
//             {
//                 Date = new DateTime(2024, 9, 21),
//                 StartTime = new TimeSpan(11, 00, 00),
//                 ClientId = 6, // Lucia Fernandes
//                 BarberShopId = 6, // Barbearia da Serena
//                 Services = new List<Service>
//                 {
//                     new Service { Name = "Corte Serena", Duration = new TimeSpan(0, 50, 0), Price = 65M }
//                 },
//                 Notes = "Cliente pediu corte de estilo tradicional."
//             },
//             new Appointment
//             {
//                 Date = new DateTime(2024, 9, 22),
//                 StartTime = new TimeSpan(15, 30, 00),
//                 ClientId = 7, // André Silva
//                 BarberShopId = 1, // Barbearia do Messi
//                 Services = new List<Service>
//                 {
//                     new Service { Name = "Barba Messi", Duration = new TimeSpan(0, 20, 0), Price = 30M }
//                 },
//                 Notes = "Barba alinhada para evento."
//             },
//             new Appointment
//             {
//                 Date = new DateTime(2024, 9, 22),
//                 StartTime = new TimeSpan(16, 00, 00),
//                 ClientId = 8, // Gabriel Martins
//                 BarberShopId = 2, // Barbearia do CR7
//                 Services = new List<Service>
//                 {
//                     new Service { Name = "Barba CR7", Duration = new TimeSpan(0, 30, 0), Price = 35M }
//                 },
//                 Notes = "Cliente quer barba estilizada."
//             },
//             new Appointment
//             {
//                 Date = new DateTime(2024, 9, 23),
//                 StartTime = new TimeSpan(09, 30, 00),
//                 ClientId = 9, // Fernanda Costa
//                 BarberShopId = 3, // Barbearia do Neymar
//                 Services = new List<Service>
//                 {
//                     new Service { Name = "Barba Neymar", Duration = new TimeSpan(0, 25, 0), Price = 40M }
//                 },
//                 Notes = "Barba para ocasião especial."
//             },
//             new Appointment
//             {
//                 Date = new DateTime(2024, 9, 23),
//                 StartTime = new TimeSpan(12, 00, 00),
//                 ClientId = 10, // Eduardo Souza
//                 BarberShopId = 4, // Barbearia da Rosamaria
//                 Services = new List<Service>
//                 {
//                     new Service { Name = "Escova Rosa", Duration = new TimeSpan(0, 25, 0), Price = 45M }
//                 },
//                 Notes = "Cliente pediu escova modeladora."
//             },
//             new Appointment
//             {
//                 Date = new DateTime(2024, 9, 24),
//                 StartTime = new TimeSpan(13, 00, 00),
//                 ClientId = 11, // Patricia Silva
//                 BarberShopId = 5, // Barbearia da Alex
//                 Services = new List<Service>
//                 {
//                     new Service { Name = "Escova Alex", Duration = new TimeSpan(0, 35, 0), Price = 60M }
//                 },
//                 Notes = "Escova para casamento."
//             },
//             new Appointment
//             {
//                 Date = new DateTime(2024, 9, 24),
//                 StartTime = new TimeSpan(14, 00, 00),
//                 ClientId = 12, // Leonardo Lima
//                 BarberShopId = 6, // Barbearia da Serena
//                 Services = new List<Service>
//                 {
//                     new Service { Name = "Escova Serena", Duration = new TimeSpan(0, 30, 0), Price = 55M }
//                 },
//                 Notes = "Cliente pediu estilo natural."
//             },
//             new Appointment
//             {
//                 Date = new DateTime(2024, 9, 25),
//                 StartTime = new TimeSpan(09, 00, 00),
//                 ClientId = 13, // Juliana Almeida
//                 BarberShopId = 1, // Barbearia do Messi
//                 Services = new List<Service>
//                 {
//                     new Service { Name = "Corte Messi", Duration = new TimeSpan(0, 40, 0), Price = 50M }
//                 },
//                 Notes = "Corte tradicional do Messi."
//             },
//             new Appointment
//             {
//                 Date = new DateTime(2024, 9, 25),
//                 StartTime = new TimeSpan(10, 30, 00),
//                 ClientId = 14, // Rodrigo Souza
//                 BarberShopId = 2, // Barbearia do CR7
//                 Services = new List<Service>
//                 {
//                     new Service { Name = "Corte CR7", Duration = new TimeSpan(0, 45, 0), Price = 60M }
//                 },
//                 Notes = "Cliente pediu corte personalizado."
//             },
//             new Appointment
//             {
//                 Date = new DateTime(2024, 9, 26),
//                 StartTime = new TimeSpan(11, 00, 00),
//                 ClientId = 15, // Renata Santos
//                 BarberShopId = 3, // Barbearia do Neymar
//                 Services = new List<Service>
//                 {
//                     new Service { Name = "Corte Neymar", Duration = new TimeSpan(0, 50, 0), Price = 70M }
//                 },
//                 Notes = "Cliente pediu estilo ousado."
//             },
//             new Appointment
//             {
//                 Date = new DateTime(2024, 9, 26),
//                 StartTime = new TimeSpan(12, 30, 00),
//                 ClientId = 16, // Marcelo Carvalho
//                 BarberShopId = 4, // Barbearia da Rosamaria
//                 Services = new List<Service>
//                 {
//                     new Service { Name = "Escova Rosa", Duration = new TimeSpan(0, 25, 0), Price = 45M }
//                 },
//                 Notes = "Cliente pediu escova com volume."
//             },
//             new Appointment
//             {
//                 Date = new DateTime(2024, 9, 27),
//                 StartTime = new TimeSpan(14, 00, 00),
//                 ClientId = 17, // Lucas Pereira
//                 BarberShopId = 5, // Barbearia da Alex
//                 Services = new List<Service>
//                 {
//                     new Service { Name = "Barba Alex", Duration = new TimeSpan(0, 20, 0), Price = 40M }
//                 },
//                 Notes = "Cliente quer barba baixa."
//             },
//             new Appointment
//             {
//                 Date = new DateTime(2024, 9, 28),
//                 StartTime = new TimeSpan(15, 30, 00),
//                 ClientId = 18, // Diego Ferreira
//                 BarberShopId = 6, // Barbearia da Serena
//                 Services = new List<Service>
//                 {
//                     new Service { Name = "Barba Serena", Duration = new TimeSpan(0, 30, 0), Price = 50M }
//                 },
//                 Notes = "Barba para evento formal."
//             },
//             new Appointment
//             {
//                 Date = new DateTime(2024, 9, 28),
//                 StartTime = new TimeSpan(10, 00, 00),
//                 ClientId = 19, // Bianca Souza
//                 BarberShopId = 1, // Barbearia do Messi
//                 Services = new List<Service>
//                 {
//                     new Service { Name = "Escova Messi", Duration = new TimeSpan(0, 35, 0), Price = 60M }
//                 },
//                 Notes = "Cliente quer escova com brilho."
//             },
//             new Appointment
//             {
//                 Date = new DateTime(2024, 9, 29),
//                 StartTime = new TimeSpan(09, 00, 00),
//                 ClientId = 20, // Marina Oliveira
//                 BarberShopId = 2, // Barbearia do CR7
//                 Services = new List<Service>
//                 {
//                     new Service { Name = "Escova CR7", Duration = new TimeSpan(0, 40, 0), Price = 65M }
//                 },
//                 Notes = "Cliente quer escova reta."
//             }
//         };

//         dbContext.Appointments.AddRange(appointments);
//         await dbContext.SaveChangesAsync();
//     }

// }
