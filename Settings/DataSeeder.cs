using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Settings;

public static class DataSeeder
{
    // public static async Task ClearAllRowsBeforeSeedAsync(IServiceProvider serviceProvider)
    // {
    //     var context = serviceProvider.GetRequiredService<AppDbContext>();
    //     using var transaction = await context.Database.BeginTransactionAsync();

    //     try
    //     {
    //         await context.Messages
    //             .IgnoreQueryFilters().ExecuteDeleteAsync();
    //         await context.Reports
    //             .IgnoreQueryFilters().ExecuteDeleteAsync();

    //         await context.Appointments
    //             .IgnoreQueryFilters().ExecuteDeleteAsync();
    //         await context.Services
    //             .IgnoreQueryFilters().ExecuteDeleteAsync();
            
    //         await context.SpecialSchedules
    //             .IgnoreQueryFilters().ExecuteDeleteAsync();
    //         await context.RecurringSchedules
    //             .IgnoreQueryFilters().ExecuteDeleteAsync();
    //         await context.Addresses
    //             .IgnoreQueryFilters().ExecuteDeleteAsync();
    //         await context.BarberShops
    //             .IgnoreQueryFilters().ExecuteDeleteAsync();
            
    //         await context.Profiles
    //             .IgnoreQueryFilters()
    //             // .Where(p => p.User.Email != "diogols@live.com")
    //             .ExecuteDeleteAsync();

    //         await context.Users
    //             .IgnoreQueryFilters()
    //             // .Where(u => u.Email != "diogols@live.com")
    //             .ExecuteDeleteAsync();
                
    //         await transaction.CommitAsync();
    //     }
    //     catch (Exception)
    //     {
    //         // Rollback em caso de falha
    //         await transaction.RollbackAsync();
    //         throw;
    //     }
    // }
    
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

        // await UpdateImageUrlsForFirstTime(serviceProvider);
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

    private static HashSet<string> GetAdminRoles() => [nameof(UserRole.Guest), nameof(UserRole.Client), nameof(UserRole.Admin)];
    
    public static HashSet<string> GetUserRolesToBeSetted(User user)
    {
        if (user.Email == "diogols@live.com" || user.Email!.StartsWith("admin"))
            return GetAdminRoles();
        
        HashSet<string> roles = [nameof(UserRole.Guest)];

        if (user.Profile is not null)
        {
            roles.Add(nameof(UserRole.Client));
            
            if (user.BarberShop is not null)
                roles.Add(nameof(UserRole.BarberShop));
        }
        
        return roles;
    }
    
    public static User[] GetAllUsersToMock() =>
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
                            new(2025, 01, 01),
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
                            new(2024, 12, 12),
                            null,
                            new(09, 00, 00),
                            new(13, 00, 00),
                            false
                        ),
                        new(
                            new(2024, 12, 24),
                            "Véspera Natal",
                            null,
                            null,
                            true
                        ),
                        new(
                            new(2025, 12, 24),
                            "Natal",
                            null,
                            null,
                            true
                        ),
                        new(
                            new(2025, 12, 25),
                            "Véspera Ano Novo",
                            null,
                            null,
                            true
                        ),
                        new(
                            new(2025, 01, 01),
                            "Ano Novo",
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
                        new(
                            "Drible no PES 2024",
                            "Neymar te ensina como fazer os melhores dribles no PES 2024 para você humilhar todos seus amigos",
                            12.49M,
                            new(00, 15, 00)
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
                            new(2024, 12, 12),
                            null,
                            new(08, 00, 00),
                            new(12, 00, 00),
                            false
                        ),
                        new(
                            new(2024, 12, 24),
                            null,
                            new(09, 00, 00),
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
                        new(
                            new(2025, 01, 01),
                            "Ano Novo",
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
                        new(
                            "Conversa com a Rosa",
                            "Para quem deseja passar momentos muito especiais com a Rosa, para conversar, jogar, desabafar, ver um filme...",
                            96.9M,
                            new(01, 02, 00)
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
                            new(2024, 12, 12),
                            "Imaculada Conceição",
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
                            new(2024, 12, 25),
                            "Natal",
                            null,
                            null,
                            true
                        ),
                        new(
                            new(2024, 12, 26),
                            "Viagem",
                            null,
                            null,
                            true
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
