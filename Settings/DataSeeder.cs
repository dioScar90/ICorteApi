using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Settings;

public static class DataSeeder
{
    // public static async Task ClearAllRowsBeforeSeedAsync(IServiceProvider serviceProvider)
    // {
    //     var context = serviceProvider.GetRequiredService<AppDbContext>();

    //     // Desabilitar as constraints antes de limpar as tabelas (opcional)
    //     await context.Database.ExecuteSqlRawAsync("EXEC sp_MSforeachtable @command1 = 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'");

    //     // Executar DELETE nas tabelas mapeadas
    //     await context.Database.ExecuteSqlRawAsync("DELETE FROM messages");
    //     await context.Database.ExecuteSqlRawAsync("DELETE FROM reports");
    //     await context.Database.ExecuteSqlRawAsync("DELETE FROM service_appointment");
    //     await context.Database.ExecuteSqlRawAsync("DELETE FROM services");
    //     await context.Database.ExecuteSqlRawAsync("DELETE FROM appointments");
    //     await context.Database.ExecuteSqlRawAsync("DELETE FROM recurring_schedules");
    //     await context.Database.ExecuteSqlRawAsync("DELETE FROM special_schedules");
    //     await context.Database.ExecuteSqlRawAsync("DELETE FROM addresses");
    //     await context.Database.ExecuteSqlRawAsync("DELETE FROM barber_shops");
    //     await context.Database.ExecuteSqlRawAsync("DELETE FROM profiles");

    //     // AspNet Identity tables
    //     await context.Database.ExecuteSqlRawAsync("DELETE FROM AspNetRoleClaims");
    //     await context.Database.ExecuteSqlRawAsync("DELETE FROM AspNetUserClaims");
    //     await context.Database.ExecuteSqlRawAsync("DELETE FROM AspNetUserLogins");
    //     await context.Database.ExecuteSqlRawAsync("DELETE FROM AspNetUserRoles");
    //     await context.Database.ExecuteSqlRawAsync("DELETE FROM AspNetUserTokens");
    //     await context.Database.ExecuteSqlRawAsync("DELETE FROM AspNetRoles");
    //     await context.Database.ExecuteSqlRawAsync("DELETE FROM asp_net_users");

    //     // Habilitar as constraints novamente (opcional)
    //     await context.Database.ExecuteSqlRawAsync("EXEC sp_MSforeachtable @command1 = 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL'");
    // }

    // public static async Task ClearAllRowsBeforeSeedAsync(IServiceProvider serviceProvider)
    // {
    //     var context = serviceProvider.GetRequiredService<AppDbContext>();

    //     // 1. Desativar as constraints de maneira genérica não é tão comum no PostgreSQL quanto no SQL Server.
    //     // No entanto, em alguns casos, pode-se optar por truncar as tabelas (isso depende se as constraints podem ser restauradas sem problemas).
    //     // As constraints podem ser manualmente gerenciadas, ou você pode utilizar transações.

    //     // Iniciar uma transação
    //     using var transaction = await context.Database.BeginTransactionAsync();

    //     try
    //     {
    //         // 2. Limpar as tabelas de domínio
    //         context.Messages.RemoveRange(context.Messages);
    //         context.Reports.RemoveRange(context.Reports);
    //         context.ServiceAppointments.RemoveRange(context.ServiceAppointments);
    //         context.Services.RemoveRange(context.Services);
    //         context.Appointments.RemoveRange(context.Appointments);
    //         context.RecurringSchedules.RemoveRange(context.RecurringSchedules);
    //         context.SpecialSchedules.RemoveRange(context.SpecialSchedules);
    //         context.Addresses.RemoveRange(context.Addresses);
    //         context.BarberShops.RemoveRange(context.BarberShops);
    //         context.Profiles.RemoveRange(context.Profiles);

    //         // 3. Limpar as tabelas de AspNet Identity
    //         context.AspNetRoleClaims.RemoveRange(context.AspNetRoleClaims);
    //         context.AspNetUserClaims.RemoveRange(context.AspNetUserClaims);
    //         context.AspNetUserLogins.RemoveRange(context.AspNetUserLogins);
    //         context.AspNetUserRoles.RemoveRange(context.AspNetUserRoles);
    //         context.AspNetUserTokens.RemoveRange(context.AspNetUserTokens);
    //         context.AspNetRoles.RemoveRange(context.AspNetRoles);
    //         context.AspNetUsers.RemoveRange(context.AspNetUsers);

    //         // 4. Salvar as alterações
    //         await context.SaveChangesAsync();

    //         // 5. Commitar a transação
    //         await transaction.CommitAsync();
    //     }
    //     catch
    //     {
    //         // Rollback em caso de falha
    //         await transaction.RollbackAsync();
    //         throw;
    //     }

    //     // 6. Opcional: Dependendo do comportamento das constraints, pode ser necessário reiniciar algumas delas manualmente.
    // }

    // public static async Task ClearAllRowsBeforeSeedAsync(IServiceProvider serviceProvider)
    // {
    //     var context = serviceProvider.GetRequiredService<AppDbContext>();

    //     // Iniciar uma transação
    //     using var transaction = await context.Database.BeginTransactionAsync();

    //     try
    //     {
    //         // Apagar e recriar o esquema 'public'
    //         await context.Database.ExecuteSqlRawAsync("DROP SCHEMA public CASCADE;");
    //         await context.Database.ExecuteSqlRawAsync("CREATE SCHEMA public;");
            
    //         // Restaurar permissões
    //         await context.Database.ExecuteSqlRawAsync("GRANT ALL ON SCHEMA public TO postgres;");
    //         await context.Database.ExecuteSqlRawAsync("GRANT ALL ON SCHEMA public TO public;");

    //         // Commitar a transação
    //         await transaction.CommitAsync();
    //     }
    //     catch (Exception ex)
    //     {
    //         // Rollback em caso de falha
    //         await transaction.RollbackAsync();
    //         throw;
    //     }
    // }

    public static async Task ClearAllRowsBeforeSeedAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<AppDbContext>();

        // Iniciar uma transação
        using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            // context.Messages.RemoveRange(context.Messages);
            // context.Reports.RemoveRange(context.Reports);

            // context.Appointments.RemoveRange(context.Appointments);
            // context.Services.RemoveRange(context.Services);
            
            // context.SpecialSchedules.RemoveRange(context.SpecialSchedules);
            // context.RecurringSchedules.RemoveRange(context.RecurringSchedules);
            // context.Addresses.RemoveRange(context.Addresses);
            // context.BarberShops.RemoveRange(context.BarberShops);
            
            // context.Profiles.RemoveRange(context.Profiles);

            await context.Messages.IgnoreQueryFilters().ExecuteDeleteAsync();
            await context.Reports.IgnoreQueryFilters().ExecuteDeleteAsync();

            await context.Appointments.IgnoreQueryFilters().ExecuteDeleteAsync();
            await context.Services.IgnoreQueryFilters().ExecuteDeleteAsync();
            
            await context.SpecialSchedules.IgnoreQueryFilters().ExecuteDeleteAsync();
            await context.RecurringSchedules.IgnoreQueryFilters().ExecuteDeleteAsync();
            await context.Addresses.IgnoreQueryFilters().ExecuteDeleteAsync();
            await context.BarberShops.IgnoreQueryFilters().ExecuteDeleteAsync();
            
            await context.Profiles.IgnoreQueryFilters().ExecuteDeleteAsync();

            await context.Users
                .IgnoreQueryFilters()
                .Where(u => u.Email != "diogols@live.com")
                .ExecuteDeleteAsync();

            // // Apagar e recriar o esquema 'public'
            // await context.Database.ExecuteSqlRawAsync("DROP SCHEMA public CASCADE;");
            // await context.Database.ExecuteSqlRawAsync("CREATE SCHEMA public;");
            
            // // Restaurar permissões
            // await context.Database.ExecuteSqlRawAsync("GRANT ALL ON SCHEMA public TO postgres;");
            // await context.Database.ExecuteSqlRawAsync("GRANT ALL ON SCHEMA public TO public;");

            // Commitar a transação
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            // Rollback em caso de falha
            await transaction.RollbackAsync();
            throw;
        }
    }


    
    public static async Task SeedData(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        
        var usersToMock = GetAllUsersToMock();
        
        foreach (var user in usersToMock)
        {
            // Finding if user already exists.
            if ((await userManager.FindByEmailAsync(user.Email!)) is not null)
                continue;
                
            // Trying to create a not already existed user.
            var identityResult = await userManager.CreateAsync(user, user.GetPasswordToBeHashed());

            if (!identityResult.Succeeded)
            {
                foreach (var err in identityResult.Errors)
                {
                    Console.WriteLine("Error.Code => " + err.Code);
                    Console.WriteLine("Error.Description => " + err.Description);
                }

                continue;
            }
            
            await userManager.AddToRolesAsync(user, [..GetUserRolesToBeSetted(user)]);
        }

        await UpdateImageUrlsForFirstTime(serviceProvider);
    }
    
    private static async Task UpdateImageUrlsForFirstTime(IServiceProvider serviceProvider)
    {
        string profileBaseUrl = Profile.GetProfileBaseImageUrlPlaceholder();
        string barberShopBaseUrl = BarberShop.GetBarberShopBaseImageUrlPlaceholder();
        
        var db = serviceProvider.GetRequiredService<AppDbContext>();
        
        await db.Profiles
            .Where(p => p.ImageUrl == null)
            .ExecuteUpdateAsync(setters => setters.SetProperty(p => p.ImageUrl,
                p => profileBaseUrl + "/" + (p.Gender == Gender.Male ? "men" : "women") + "/" + (99 - p.Id) + ".jpg"));

        await db.BarberShops
            .Where(p => p.ImageUrl == null)
            .ExecuteUpdateAsync(setters => setters.SetProperty(p => p.ImageUrl,
                p => barberShopBaseUrl + "/" + (950 - p.OwnerId) + "/300"));
    }
    
    private static HashSet<string> GetUserRolesToBeSetted(User user)
    {
        if (user.Email == "diogols@live.com" || user.Email!.StartsWith("admin"))
            return [nameof(UserRole.Admin)];
        
        HashSet<string> roles = [nameof(UserRole.Guest)];

        if (user.Profile is not null)
        {
            roles.Add(nameof(UserRole.Client));
            
            if (user.BarberShop is not null)
                roles.Add(nameof(UserRole.BarberShop));
        }
        
        return roles;
    }
    
    private static User[] GetAllUsersToMock() =>
        [
            ..GetAdminUsers(),
            ..GetBarberShopUsers(),
            ..GetClientUsers(),
        ];

    private static User[] GetAdminUsers() =>
        [
            new(
                new(
                    "diogols@live.com", "Dunno#2024",
                    new("Diogo", "Scarmagnani", Gender.Male, "19912354698")
                )
            ),
            new(
                new(
                    "admin_2@gmail.com", "Dunno#2024",
                    new("Admin", "Dois", Gender.Male, "18995632564")
                )
            ),
        ];
        
    private static User[] GetBarberShopUsers() =>
        [
            GetBarberShopLionelMessi(),
            GetBarberShopCristianoRonaldo(),
            GetBarberShopNeymarJr(),
            GetBarberShopRosamariaMontibeller(),
            GetBarberShopAlexMorgan(),
            GetBarberShopSerenaWilliams(),
        ];

    private static User[] GetClientUsers() =>
        [
            GetClientJoao(),
            GetClientMaria(),
            GetClientCarlos(),
            GetClientAna(),
            GetClientPaulo(),
            GetClientLucia(),
            GetClientAndre(),
            GetClientGabriel(),
            GetClientFernanda(),
            GetClientEduardo(),
            GetClientPatricia(),
            GetClientLeonardo(),
            GetClientJuliana(),
            GetClientRodrigo(),
            GetClientRenata(),
            GetClientMarcelo(),
        ];
        
    private static User GetBarberShopLionelMessi() =>
        new(
            new(
                "messi@barbershop.com", "Goat#2024",
                new(
                    "Lionel", "Messi", Gender.Male, "11998765432"
                ),
                new(
                    "Barbearia do Messi", "Corte com precisão!", "11998765432", "messi@barbershop.com",
                    new(
                        "Av Argentina", "10", null, "Centro", "São Paulo", State.SP, "01000000", "Brasil"
                    ),
                    [
                        new(
                            DayOfWeek.Monday,
                            new(08, 00, 00),
                            new(18, 00, 00)
                        ),
                        new(
                            DayOfWeek.Tuesday,
                            new(08, 00, 00),
                            new(18, 00, 00)
                        ),
                        new(
                            DayOfWeek.Wednesday,
                            new(08, 00, 00),
                            new(18, 00, 00)
                        ),
                        new(
                            DayOfWeek.Thursday,
                            new(08, 00, 00),
                            new(18, 00, 00)
                        ),
                        new(
                            DayOfWeek.Friday,
                            new(08, 00, 00),
                            new(18, 00, 00)
                        ),
                        new(
                            DayOfWeek.Saturday,
                            new(09, 00, 00),
                            new(13, 00, 00)
                        ),
                    ],
                    [
                        new(
                            new(2024, 12, 24),
                            null,
                            new(08, 00, 00),
                            new(12, 00, 00),
                            false
                        ),
                        new(
                            new(2024, 12, 25),
                            "Natal",
                            null,
                            null,
                            true
                        ),
                    ],
                    [
                        new(
                            "Corte Messi",
                            "Corte estiloso do Messi",
                            50M,
                            new(00, 40, 00)
                        ),
                        new(
                            "Barba Messi",
                            "Barba bem alinhada",
                            30M,
                            new(00, 20, 00)
                        ),
                    ]
                )
            )
        );
        
    private static User GetBarberShopCristianoRonaldo() =>
        new(
            new(
                "ronaldo@barbershop.com", "Cr7#2024",
                new(
                    "Cristiano", "Ronaldo", Gender.Male, "11987654321"
                ),
                new(
                    "Barbearia do CR7", "Corte com estilo!", "11987654321", "ronaldo@barbershop.com",
                    new(
                        "R Cristiano Ronaldo", "7", null, "Jardins", "São Paulo", State.SP, "01452000", "Brasil"
                    ),
                    [
                        new(
                            DayOfWeek.Monday,
                            new(09, 00, 00),
                            new(19, 00, 00)
                        ),
                        new(
                            DayOfWeek.Tuesday,
                            new(09, 00, 00),
                            new(19, 00, 00)
                        ),
                        new(
                            DayOfWeek.Wednesday,
                            new(09, 00, 00),
                            new(19, 00, 00)
                        ),
                        new(
                            DayOfWeek.Thursday,
                            new(09, 00, 00),
                            new(19, 00, 00)
                        ),
                        new(
                            DayOfWeek.Friday,
                            new(09, 00, 00),
                            new(19, 00, 00)
                        ),
                        new(
                            DayOfWeek.Saturday,
                            new(10, 00, 00),
                            new(14, 00, 00)
                        ),
                    ],
                    [
                        new(
                            new(2024, 12, 31),
                            null,
                            new(09, 00, 00),
                            new(14, 00, 00),
                            false
                        ),
                        new(
                            new(2024, 01, 01),
                            "Ano Novo",
                            null,
                            null,
                            true
                        ),
                    ],
                    [
                        new(
                            "Corte CR7",
                            "Corte igual ao do Cristiano",
                            60M,
                            new(00, 45, 00)
                        ),
                        new(
                            "Barba CR7",
                            "Barba alinhada com perfeição",
                            35M,
                            new(00, 30, 00)
                        ),
                    ]
                )
            )
        );
        
    private static User GetBarberShopNeymarJr() =>
        new(
            new(
                "neymar@barbershop.com", "NeymarJr#2024",
                new(
                    "Neymar", "Jr.", Gender.Male, "11976543210"
                ),
                new(
                    "Barbearia do Neymar", "Estilo e ousadia!", "11976543210", "neymar@barbershop.com",
                    new(
                        "R Neymar", "10", null, "Vila Madalena", "São Paulo", State.SP, "05434000", "Brasil"
                    ),
                    [
                        new(
                            DayOfWeek.Monday,
                            new(10, 00, 00),
                            new(18, 00, 00)
                        ),
                        new(
                            DayOfWeek.Tuesday,
                            new(10, 00, 00),
                            new(18, 00, 00)
                        ),
                        new(
                            DayOfWeek.Wednesday,
                            new(10, 00, 00),
                            new(18, 00, 00)
                        ),
                        new(
                            DayOfWeek.Thursday,
                            new(10, 00, 00),
                            new(18, 00, 00)
                        ),
                        new(
                            DayOfWeek.Friday,
                            new(10, 00, 00),
                            new(18, 00, 00)
                        ),
                        new(
                            DayOfWeek.Saturday,
                            new(09, 00, 00),
                            new(13, 00, 00)
                        ),
                    ],
                    [
                        new(
                            new(2024, 03, 10),
                            null,
                            new(09, 00, 00),
                            new(13, 00, 00),
                            false
                        ),
                        new(
                            new(2024, 11, 02),
                            "Finados",
                            null,
                            null,
                            true
                        ),
                    ],
                    [
                        new(
                            "Corte Neymar",
                            "Corte ousado do Neymar",
                            70M,
                            new(00, 50, 00)
                        ),
                        new(
                            "Barba Neymar",
                            "Barba com estilo",
                            40M,
                            new(00, 25, 00)
                        ),
                    ]
                )
            )
        );
        
    private static User GetBarberShopRosamariaMontibeller() =>
        new(
            new(
                "rosamaria@barbershop.com", "Rosa#2024",
                new(
                    "Rosamaria", "Montibeller", Gender.Female, "11987651234"
                ),
                new(
                    "Barbearia da Rosamaria", "Beleza e força!", "11987651234", "rosamaria@barbershop.com",
                    new(
                        "R Rosamaria", "12", null, "Morumbi", "São Paulo", State.SP, "05724000", "Brasil"
                    ),
                    [
                        new(
                            DayOfWeek.Monday,
                            new(08, 00, 00),
                            new(17, 00, 00)
                        ),
                        new(
                            DayOfWeek.Tuesday,
                            new(08, 00, 00),
                            new(17, 00, 00)
                        ),
                        new(
                            DayOfWeek.Wednesday,
                            new(08, 00, 00),
                            new(17, 00, 00)
                        ),
                        new(
                            DayOfWeek.Thursday,
                            new(08, 00, 00),
                            new(17, 00, 00)
                        ),
                        new(
                            DayOfWeek.Friday,
                            new(08, 00, 00),
                            new(17, 00, 00)
                        ),
                        new(
                            DayOfWeek.Saturday,
                            new(09, 00, 00),
                            new(14, 00, 00)
                        ),
                    ],
                    [
                        new(
                            new(2024, 07, 09),
                            null,
                            new(08, 00, 00),
                            new(12, 00, 00),
                            false
                        ),
                        new(
                            new(2024, 11, 15),
                            "Proclamação da República",
                            null,
                            null,
                            true
                        ),
                    ],
                    [
                        new(
                            "Corte Rosa",
                            "Corte estiloso da Rosamaria",
                            55M,
                            new(00, 35, 00)
                        ),
                        new(
                            "Escova Rosa",
                            "Escova modeladora",
                            45M,
                            new(00, 25, 00)
                        ),
                    ]
                )
            )
        );
        
    private static User GetBarberShopAlexMorgan() =>
        new(
            new(
                "alex@barbershop.com", "Alex#2024",
                new(
                    "Alex", "Morgan", Gender.Female, "11987654321"
                ),
                new(
                    "Barbearia da Alex", "Elegância e atitude!", "11987654321", "alex@barbershop.com",
                    new(
                        "R Alex", "15", null, "Jardins", "São Paulo", State.SP, "01402000", "Brasil"
                    ),
                    [
                        new(
                            DayOfWeek.Monday,
                            new(10, 00, 00),
                            new(19, 00, 00)
                        ),
                        new(
                            DayOfWeek.Tuesday,
                            new(10, 00, 00),
                            new(19, 00, 00)
                        ),
                        new(
                            DayOfWeek.Wednesday,
                            new(10, 00, 00),
                            new(19, 00, 00)
                        ),
                        new(
                            DayOfWeek.Thursday,
                            new(10, 00, 00),
                            new(19, 00, 00)
                        ),
                        new(
                            DayOfWeek.Friday,
                            new(10, 00, 00),
                            new(19, 00, 00)
                        ),
                        new(
                            DayOfWeek.Saturday,
                            new(10, 00, 00),
                            new(14, 00, 00)
                        ),
                    ],
                    [
                        new(
                            new(2024, 05, 12),
                            "Dia das Mães",
                            null,
                            null,
                            true
                        ),
                        new(
                            new(2024, 12, 25),
                            "Natal",
                            null,
                            null,
                            true
                        ),
                    ],
                    [
                        new(
                            "Corte Alex",
                            "Corte moderno da Alex",
                            70M,
                            new(00, 45, 00)
                        ),
                        new(
                            "Escova Alex",
                            "Escova com volume",
                            60M,
                            new(00, 35, 00)
                        ),
                    ]
                )
            )
        );
        
    private static User GetBarberShopSerenaWilliams() =>
        new(
            new(
                "serena@barbershop.com", "Serena#2024",
                new(
                    "Serena", "Williams", Gender.Female, "11987653456"
                ),
                new(
                    "Barbearia da Serena", "Força e estilo!", "11987653456", "serena@barbershop.com",
                    new(
                        "R Serena", "23", null, "Bela Vista", "São Paulo", State.SP, "01322000", "Brasil"
                    ),
                    [
                        new(
                            DayOfWeek.Monday,
                            new(08, 00, 00),
                            new(17, 00, 00)
                        ),
                        new(
                            DayOfWeek.Tuesday,
                            new(08, 00, 00),
                            new(17, 00, 00)
                        ),
                        new(
                            DayOfWeek.Wednesday,
                            new(08, 00, 00),
                            new(17, 00, 00)
                        ),
                        new(
                            DayOfWeek.Thursday,
                            new(08, 00, 00),
                            new(17, 00, 00)
                        ),
                        new(
                            DayOfWeek.Friday,
                            new(08, 00, 00),
                            new(17, 00, 00)
                        ),
                        new(
                            DayOfWeek.Saturday,
                            new(09, 00, 00),
                            new(13, 00, 00)
                        ),
                    ],
                    [
                        new(
                            new(2024, 08, 22),
                            null,
                            new(08, 00, 00),
                            null,
                            false
                        ),
                        new(
                            new(2024, 12, 31),
                            "Ano Novo",
                            null,
                            null,
                            true
                        ),
                    ],
                    [
                        new(
                            "Corte Serena",
                            "Corte poderoso da Serena",
                            65M,
                            new(00, 50, 00)
                        ),
                        new(
                            "Escova Serena",
                            "Escova modeladora",
                            55M,
                            new(00, 30, 00)
                        ),
                    ]
                )
            )
        );
        
    private static User GetClientJoao() =>
        new(
            new(
                "joao.silva@gmail.com", "Password123!",
                new("João", "Silva", Gender.Male, "11987654321")
            )
        );

    private static User GetClientMaria() =>
        new(
            new(
                "maria.oliveira@gmail.com", "Password123!",
                new("Maria", "Oliveira", Gender.Female, "11976543210")
            )
        );

    private static User GetClientCarlos() =>
        new(
            new(
                "carlos.souza@gmail.com", "Password123!",
                new("Carlos", "Souza", Gender.Male, "11965432109")
            )
        );

    private static User GetClientAna() =>
        new(
            new(
                "ana.santos@gmail.com", "Password123!",
                new("Ana", "Santos", Gender.Female, "11954321098")
            )
        );

    private static User GetClientPaulo() =>
        new(
            new(
                "paulo.almeida@gmail.com", "Password123!",
                new("Paulo", "Almeida", Gender.Male, "11943210987")
            )
        );

    private static User GetClientLucia() =>
        new(
            new(
                "lucia.fernandes@gmail.com", "Password123!",
                new("Lucia", "Fernandes", Gender.Female, "11932109876")
            )
        );

    private static User GetClientAndre() =>
        new(
            new(
                "andre.pereira@gmail.com", "Password123!",
                new("André", "Pereira", Gender.Male, "11921098765")
            )
        );
        
    private static User GetClientGabriel() =>
        new(
            new(
                "gabriel.lima@gmail.com", "Password123!",
                new("Gabriel", "Lima", Gender.Male, "11912345678")
            )
        );
        
    private static User GetClientFernanda() =>
        new(
            new(
                "fernanda.costa@gmail.com", "Password123!",
                new("Fernanda", "Costa", Gender.Female, "11987654322")
            )
        );
        
    private static User GetClientEduardo() =>
        new(
            new(
                "eduardo.gomes@gmail.com", "Password123!",
                new("Eduardo", "Gomes", Gender.Male, "11911223344")
            )
        );
        
    private static User GetClientPatricia() =>
        new(
            new(
                "patricia.rocha@gmail.com", "Password123!",
                new("Patrícia", "Rocha", Gender.Female, "11955667788")
            )
        );
        
    private static User GetClientLeonardo() =>
        new(
            new(
                "leonardo.barros@gmail.com", "Password123!",
                new("Leonardo", "Barros", Gender.Male, "11933445566")
            )
        );
        
    private static User GetClientJuliana() =>
        new(
            new(
                "juliana.silveira@gmail.com", "Password123!",
                new("Juliana", "Silveira", Gender.Female, "11966778899")
            )
        );
        
    private static User GetClientRodrigo() =>
        new(
            new(
                "rodrigo.santos@gmail.com", "Password123!",
                new("Rodrigo", "Santos", Gender.Male, "11977889900")
            )
        );
        
    private static User GetClientRenata() =>
        new(
            new(
                "renata.carvalho@gmail.com", "Password123!",
                new("Renata", "Carvalho", Gender.Female, "11988990011")
            )
        );
        
    private static User GetClientMarcelo() =>
        new(
            new(
                "marcelo.ribeiro@gmail.com", "Password123!",
                new("Marcelo", "Ribeiro", Gender.Male, "11999001122")
            )
        );
}
