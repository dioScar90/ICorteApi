# iCorte

This is my personal repository about my TCC (Final Paper) project. It is a barbershop scheduling web app.
- Backend: **ASP.NET Core**, with Minimal API.
- Frontend: **React**, with React Router or Remix (I don't know...).
- Database: **SQL Server**, with Entity Framework ORM.

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

### For _dotnet-alias.sh_ (Linux only):
- Create in this root directory a file named _dotnet-alias.sh_ and write this code inside it:

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

- Change `CURRENT_DIRECTORY` variable to this current path. For example: `/c/xampp/htdocs/icorte` (if using Bash terminals on Windows) or `/home/user/icorte` (my use case, once I use Linux through WSL).
- `chmod +x dotnet-alias.sh`.
- Create an `alias` in your `.bashrc` or `.zshrc` etc. file like this: `alias dotnet="{{your_current_directory}}"` (replace for your current directory itself, of course).
- Restart your shell configuration by typing `source ~/.bashrc` or `source ~/.zshrc`, whatever.

## Some React common steps:

### Create the React Router or Remix project:

- With **React Router**:
  - `npm create vite@latest client-app -- --template react-ts`
  - `npm install react-router-dom`
- with **Remix**:
  - `npx create-remix@latest`

Some good libs to install (if needed):
- `npm i sass`
- `npm install @mui/material @emotion/react @emotion/styled`
- `npm i @mui/icons-material`
- `npm install axios`
- `npm i sweetalert2`
- npx shadcn-ui@latest init
  - Style: New York.
  - Base color: Zinc.
  - CSS variables: yes.
- npx shadcn-ui@latest add button
- npx shadcn-ui@latest add progress
- npx shadcn-ui@latest add toast
- npx shadcn-ui@latest add aspect-ratio
- npx shadcn-ui@latest add scroll-area
- npx shadcn-ui@latest add label
- npx shadcn-ui@latest add dropdown-menu
- npx shadcn-ui@latest add dialog
- npx shadcn-ui@latest add card
- npx shadcn-ui@latest add table
- npm install lucide-react
- npm install framer-motion
- npm install react-dropzone
- npm install uploadthing @uploadthing/react
- npm install zod
- npm install prisma @prisma/client
  - npx prisma init
  - npx prisma db push
  - npx prisma studio
- npm install sharp@0.32.6
- npm install react-rnd
- npm install @headlessui/react
- npm install @tanstack/react-query
- npm install react-dom-confetti
- npm install stripe
- npm install @react-email/components
- npm install resend

### Once the React project is created, on the root of this repository:
- `npm init`
- In _package.json_, _scripts_, add:
  - _"start": "concurrently \"dotnet watch run\" \"cd client-app && npm run dev\""_
- `npm install --sav-dev concurrently`
- `npm install react react-dom next --sav-dev concurrently`

Now, to run together, just type `npm install` and both backend and frontend are going to start together.
