# iCorte

## Etapas comuns:

### Criar ASP.NET Core Web API:

- `dotnet new web -n ICorteApi`

### Instalar Entity Framework:

- `dotnet tool install --global dotnet-ef` ou `dotnet tool update --global dotnet-ef`
  - `dotnet add package Microsoft.EntityFrameworkCore.Sqlite` ou
  - `dotnet add package Microsoft.EntityFrameworkCore.SqlServer` ou
  - `dotnet add package Pomelo.EntityFrameworkCore.MySql`
- `dotnet add package Microsoft.EntityFrameworkCore.Design`
- `dotnet add package Microsoft.EntityFrameworkCore.Tools`

- `dotnet add package Microsoft.AspNetCore.Authentication`
- `dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer`

- `dotnet add package Swashbuckle.AspNetCore`

### Instalar AutoMapper:

- `dotnet add package AutoMapper`
- `dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection`
  <!-- - `dotnet add package Microsoft.AspNetCore.Session` -->
  <!-- - `dotnet add package Microsoft.Extensions.DependencyInjection` -->

### Realizar Migrations:

- `dotnet ef migrations add InitialCreate`
- `dotnet ef database update`

## For _dotnet-alias.sh_

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

Another utils becoming from another project:

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

### Após criar React, agora na raiz do projeto:

- `npm init`
- Acrescentar em _package.json_, em _scripts_, o item:
  - _"start": "concurrently \"dotnet watch run\" \"cd client-app && npm run dev\""_
- `npm install --sav-dev concurrently`
- `npm install react react-dom next --sav-dev concurrently`

Agora, para rodar, basta digitar `npm start` que já irá rodar tanto o backend quanto o frontend.
