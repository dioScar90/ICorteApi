using BarberAppApi.Entities;
using BarberAppApi.Enums;

namespace BarberAppApi.Repositories;

public static class UserRepository
{
    public static async Task<User?> Get(string username, string password)
    {
        List<User> users = [
            new() { Id = 1, Username = "batman", Password = "1234", Role = Role.Admin },
            new() { Id = 2, Username = "robin", Password = "1234", Role = Role.Regular },
        ];
        
        return await Task.Run(() => users
            .FirstOrDefault(u =>
                string.Equals(username, u.Username, StringComparison.CurrentCultureIgnoreCase) &&
                u.Password == password)
            );
    }
}
