# React + Vite
## Etapas comuns:

### Criar ASP.NET Core Web API:
- `dotnet new web -n BarberAppApi`

### Instalar Entity Framework:
- `dotnet tool install --global dotnet-ef` ou `dotnet tool update --global dotnet-ef`
    - `dotnet add package Microsoft.EntityFrameworkCore.Sqlite` ou
    - `dotnet add package Microsoft.EntityFrameworkCore.SqlServer` ou
    - `dotnet add package Pomelo.EntityFrameworkCore.MySql`
- `dotnet add package Microsoft.EntityFrameworkCore.Design`
- `dotnet add package Microsoft.EntityFrameworkCore.Tools`

- `dotnet add package Microsoft.AspNetCore.Authentication`
- `dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer`

### Instalar AutoMapper:
- `dotnet add package AutoMapper`
- `dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection`
<!-- - `dotnet add package Microsoft.AspNetCore.Session` -->
<!-- - `dotnet add package Microsoft.Extensions.DependencyInjection` -->

### Realizar Migrations:
- `dotnet ef migrations add InitialCreate`
- `dotnet ef database update`

## For *dotnet-alias.sh*
- Create in this root directory a file named *dotnet-alias.sh* and write this code inside it:
```
#!/bin/bash

DOTNET_CSPROJ_PATH="./ICorteApi/ICorteApi.csproj"
CURRENT_DIRECTORY="{{your_current_directory}}"

DIRECTORY_WHERE_DOTNET_WAS_TYPPED=$(pwd)

if [ "$DIRECTORY_WHERE_DOTNET_WAS_TYPPED" == "$CURRENT_DIRECTORY" ]; then
    if [ "$1" == "add" ]; then
        shift
        dotnet add $DOTNET_CSPROJ_PATH "$@"
    elif [ "$1" == "build" ]; then
        dotnet build $DOTNET_CSPROJ_PATH
    elif [ "$1" == "run" ]; then
        dotnet run --project $DOTNET_CSPROJ_PATH
    else
        dotnet "$@"
    fi
else
    dotnet "$@"
fi
```
- Change `CURRENT_DIRECTORY` variable to this current path. For example: `/c/xampp/htdocs/icorte`.
- `chmod +x dotnet-alias.sh`.
- Create an `alias` in your `.bashrc`, `.zshrc`, etc. file like this: `alias dotnet="{{your_current_directory}}"` (replace for your current directory itself, of course).
- Restart your shell configuration by typing `source ~/.bashrc` or `source ~/.zshrc`, whatever.

### Criar projeto React.js com Next.js:
- Com **React**:
    - `npx create-react-app client-app --template typescript`
- Com **Next**:
    - `npx create-next-app@latest client-app`
        - TypeScript YES
        - ESLint YES
        - Tailwind CSS YES
        - 'src/' directory YES
        - App Router YES
        - import alias NO
- Com **Vite**:
    - `npm create vite@latest client-app -- --template react-ts`
- Instalando SASS (usar com arquivos SCSS):
    - `npm i sass`
- Instalando Material-UI e Material-Icons:
    - `npm install @mui/material @emotion/react @emotion/styled`
    - `npm i @mui/icons-material`
- Rotas:
    - `npm i react-router-dom`
- Axios:
    - `npm install axios`
- Moment:
    - `npm i moment`
- sweetalert2:
    - `npm i sweetalert2`

### Após criar React, agora na raiz do projeto:
- `npm init`
- Acrescentar em *package.json*, em *scripts*, o item:
    - *"start": "concurrently \"dotnet watch run\" \"cd client-app && npm run dev\""*
- `npm install --sav-dev concurrently`
- `npm install react react-dom next --sav-dev concurrently`

Agora, para rodar, basta digitar `npm start` que já irá rodar tanto o backend quanto o frontend.
