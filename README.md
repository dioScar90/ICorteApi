# iCorte

This is my personal repository about my TCC (Final Paper) project. It is a barbershop scheduling web app.
- Backend: **ASP.NET Core**, with Minimal API.
  - Database: **Postgres**, with Entity Framework ORM.
- Frontend: **React**, with React Router ([check this out here](https://github.com/dioScar90/icorte-app)).

## Some .NET common steps:

### Create the ASP.NET Core Minimal API project:

- `dotnet new web -n ICorteApi`

### Some packages to be installed:
- `dotnet tool install --global dotnet-ef` or `dotnet tool update --global dotnet-ef`
- `dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL` or `dotnet add package Microsoft.EntityFrameworkCore.SqlServer`
- `dotnet add package Microsoft.EntityFrameworkCore.Design`
- `dotnet add package Microsoft.EntityFrameworkCore.Tools`
- `dotnet add package Microsoft.AspNetCore.Authentication`
- `dotnet add package Microsoft.AspNetCore.Identity`
- `dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore`
- `dotnet add package Microsoft.AspNetCore.Identity.UI`
- `dotnet add package FluentValidation.AspNetCore`
- `dotnet add package Swashbuckle.AspNetCore`

### Migrations:
- `dotnet ef migrations add {{Message}}`
- `dotnet ef database update`
