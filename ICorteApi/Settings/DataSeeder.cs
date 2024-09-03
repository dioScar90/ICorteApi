using System.Text.Json;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Settings;

public static class DataSeeder
{
    public static async Task SeedData(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

        // Check is there is already some valid user inserted.
        if (await userManager.Users.AnyAsync(u => !u.IsDeleted))
            return;
        
        var usersWithRoles = GetAllUsersToMock();

        foreach (var (user, userRoles) in usersWithRoles)
        {
            // Finding if user already exists.
            if ((await userManager.FindByEmailAsync(user.Email!)) is not null)
                continue;

            // Trying to create a not already existed user.
            if (!(await userManager.CreateAsync(user, user.GetPasswordToBeHashed())).Succeeded)
                continue;

            // Seeding roles to user just created.
            var rolesAsString = userRoles.Select(role => role.ToString());
            await userManager.AddToRolesAsync(user, [..rolesAsString]);
        }
    }
    
    private static (User, UserRole[])[] GetAllUsersToMock() =>
        [
            ..GetAdminUsers(),
            ..GetBarberShopUsers(),
            ..GetClientUsers(),
        ];

    private static (User, UserRole[])[] GetAdminUsers() =>
        [
            (
                new(
                    new(
                        "diogols@live.com", "Dunno#2024",
                        new("Diogo", "Scarmagnani", Gender.Male, "19912354698")
                    )
                ),
                [UserRole.Admin]
            ),
            (
                new(
                    new(
                        "admin_2@gmail.com", "Dunno#2024",
                        new("Admin", "Dois", Gender.Male, "18995632564")
                    )
                ),
                [UserRole.Admin]
            ),
        ];
        
    private static (User, UserRole[])[] GetBarberShopUsers() =>
        [
            GetBarberShopLionelMessi(),
            GetBarberShopCristianoRonaldo(),
            GetBarberShopNeymarJr(),
            GetBarberShopRosamariaMontibeller(),
            GetBarberShopAlexMorgan(),
            GetBarberShopSerenaWilliams(),
        ];

    private static (User, UserRole[])[] GetClientUsers() =>
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
        
    private static (User, UserRole[]) GetBarberShopLionelMessi() =>
        (
            new(
                new(
                    "messi@barbershop.com", "GOAT2024",
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
            ),
            [UserRole.Guest, UserRole.Client, UserRole.BarberShop]
        );
        
    private static (User, UserRole[]) GetBarberShopCristianoRonaldo() =>
        (
            new(
                new(
                    "ronaldo@barbershop.com", "CR72024",
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
            ),
            [UserRole.Guest, UserRole.Client, UserRole.BarberShop]
        );
        
    private static (User, UserRole[]) GetBarberShopNeymarJr() =>
        (
            new(
                new(
                    "neymar@barbershop.com", "NeymarJr2024",
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
            ),
            [UserRole.Guest, UserRole.Client, UserRole.BarberShop]
        );
        
    private static (User, UserRole[]) GetBarberShopRosamariaMontibeller() =>
        (
            new(
                new(
                    "rosamaria@barbershop.com", "Rosa2024",
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
            ),
            [UserRole.Guest, UserRole.Client, UserRole.BarberShop]
        );
        
    private static (User, UserRole[]) GetBarberShopAlexMorgan() =>
        (
            new(
                new(
                    "alex@barbershop.com", "Alex2024",
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
            ),
            [UserRole.Guest, UserRole.Client, UserRole.BarberShop]
        );

        
    private static (User, UserRole[]) GetBarberShopSerenaWilliams() =>
        (
            new(
                new(
                    "serena@barbershop.com", "Serena2024",
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
            ),
            [UserRole.Guest, UserRole.Client, UserRole.BarberShop]
        );





    
        
    private static (User, UserRole[]) GetClientJoao() =>
        (
            new(
                new(
                    "joao.silva@gmail.com", "Password123!",
                    new("João", "Silva", Gender.Male, "11987654321")
                )
            ),
            [UserRole.Guest, UserRole.Client]
        );

    private static (User, UserRole[]) GetClientMaria() =>
        (
            new(
                new(
                    "maria.oliveira@gmail.com", "Password123!",
                    new("Maria", "Oliveira", Gender.Female, "11976543210")
                )
            ),
            [UserRole.Guest, UserRole.Client]
        );

    private static (User, UserRole[]) GetClientCarlos() =>
        (
            new(
                new(
                    "carlos.souza@gmail.com", "Password123!",
                    new("Carlos", "Souza", Gender.Male, "11965432109")
                )
            ),
            [UserRole.Guest, UserRole.Client]
        );

    private static (User, UserRole[]) GetClientAna() =>
        (
            new(
                new(
                    "ana.santos@gmail.com", "Password123!",
                    new("Ana", "Santos", Gender.Female, "11954321098")
                )
            ),
            [UserRole.Guest, UserRole.Client]
        );

    private static (User, UserRole[]) GetClientPaulo() =>
        (
            new(
                new(
                    "paulo.almeida@gmail.com", "Password123!",
                    new("Paulo", "Almeida", Gender.Male, "11943210987")
                )
            ),
            [UserRole.Guest, UserRole.Client]
        );

    private static (User, UserRole[]) GetClientLucia() =>
        (
            new(
                new(
                    "lucia.fernandes@gmail.com", "Password123!",
                    new("Lucia", "Fernandes", Gender.Female, "11932109876")
                )
            ),
            [UserRole.Guest, UserRole.Client]
        );

    private static (User, UserRole[]) GetClientAndre() =>
        (
            new(
                new(
                    "andre.pereira@gmail.com", "Password123!",
                    new("André", "Pereira", Gender.Male, "11921098765")
                )
            ),
            [UserRole.Guest, UserRole.Client]
        );
        
    private static (User, UserRole[]) GetClientGabriel() =>
        (
            new(
                new(
                    "gabriel.lima@gmail.com", "Password123!",
                    new("Gabriel", "Lima", Gender.Male, "11912345678")
                )
            ),
            [UserRole.Guest, UserRole.Client]
        );
        
    private static (User, UserRole[]) GetClientFernanda() =>
        (
            new(
                new(
                    "fernanda.costa@gmail.com", "Password123!",
                    new("Fernanda", "Costa", Gender.Female, "11987654322")
                )
            ),
            [UserRole.Guest, UserRole.Client]
        );
        
    private static (User, UserRole[]) GetClientEduardo() =>
        (
            new(
                new(
                    "eduardo.gomes@gmail.com", "Password123!",
                    new("Eduardo", "Gomes", Gender.Male, "11911223344")
                )
            ),
            [UserRole.Guest, UserRole.Client]
        );
        
    private static (User, UserRole[]) GetClientPatricia() =>
        (
            new(
                new(
                    "patricia.rocha@gmail.com", "Password123!",
                    new("Patrícia", "Rocha", Gender.Female, "11955667788")
                )
            ),
            [UserRole.Guest, UserRole.Client]
        );
        
    private static (User, UserRole[]) GetClientLeonardo() =>
        (
            new(
                new(
                    "leonardo.barros@gmail.com", "Password123!",
                    new("Leonardo", "Barros", Gender.Male, "11933445566")
                )
            ),
            [UserRole.Guest, UserRole.Client]
        );
        
    private static (User, UserRole[]) GetClientJuliana() =>
        (
            new(
                new(
                    "juliana.silveira@gmail.com", "Password123!",
                    new("Juliana", "Silveira", Gender.Female, "11966778899")
                )
            ),
            [UserRole.Guest, UserRole.Client]
        );
        
    private static (User, UserRole[]) GetClientRodrigo() =>
        (
            new(
                new(
                    "rodrigo.santos@gmail.com", "Password123!",
                    new("Rodrigo", "Santos", Gender.Male, "11977889900")
                )
            ),
            [UserRole.Guest, UserRole.Client]
        );
        
    private static (User, UserRole[]) GetClientRenata() =>
        (
            new(
                new(
                    "renata.carvalho@gmail.com", "Password123!",
                    new("Renata", "Carvalho", Gender.Female, "11988990011")
                )
            ),
            [UserRole.Guest, UserRole.Client]
        );
        
    private static (User, UserRole[]) GetClientMarcelo() =>
        (
            new(
                new(
                    "marcelo.ribeiro@gmail.com", "Password123!",
                    new("Marcelo", "Ribeiro", Gender.Male, "11999001122")
                )
            ),
            [UserRole.Guest, UserRole.Client]
        );
}

