namespace ICorteApi;

// public interface IPerson {}
// public interface IRecord {}

// public class Person : IPerson
// {
//     public string Name { get; set; }
//     public int Age { get; set; }
// }

// public class Programmer : IPerson
// {
//     public string[] Stacks { get; set; }
//     public Person? Person { get; set; }
// }

// public record PersonRec(
//     string Name,
//     int Age
// ): IRecord;

// public record ProgrammerRec(
//     string[] Stacks,
//     PersonRec? Person
// ): IRecord;

// public static class Vai
// {
//     public static T? GetT<T>(this IPerson entity) where T : class, IRecord
//     {
//         if (entity is Person person)
//         {
//             return new PersonRec(
//                 person.Name,
//                 person.Age
//             ) as T;
//         }

//         if (entity is Programmer programmer)
//         {
//             return new ProgrammerRec(
//                 programmer.Stacks,
//                 programmer.Person is Person personnn ? personnn.GetT<PersonRec>() : null
//             ) as T;
//         }

//         return default;
//     }
// }

public class Program
{
    public static void Main(string[] args)
    {

        // // No método Main ou no Program.cs
        // using (var scope = app.Services.CreateScope())
        // {
        //     var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        //     await RoleSeeder.SeedRoles(roleManager);
        // }
        
        CreateHostBuilder(args)
            .Build()
            .Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}
