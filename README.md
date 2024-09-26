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

- Creating With **Vite**:
  - `npm create vite@latest`
    - Name: _client-app_.
    - React
    - TypeScript
  - `cd client-app`
  - `npm i`

- Installing Shadcn:
  - `npm i -D tailwindcss postcss autoprefixer`
  - `npx tailwindcss init -p`

  - Add the paths to all of your template files in your _tailwind.config.js_ file.

  ```
  /** @type {import('tailwindcss').Config} */
  export default {
    content: [
      "./index.html",
      "./src/**/*.{js,ts,jsx,tsx}",
    ],
    theme: {
      extend: {},
    },
    plugins: [],
  }
  ```

  - Add the _@tailwind_ directives for each of Tailwind’s layers to your _./src/index.css_ file.

  ```
  @tailwind base;
  @tailwind components;
  @tailwind utilities;
  ```

  - The current version of Vite splits TypeScript configuration into three files, two of which need to be edited. Add the _baseUrl_ and _paths_ properties to the _compilerOptions_ section of the _tsconfig.json_ and _tsconfig.app.json_ files:

  ```
  {
    // ...
    "compilerOptions": {
      "baseUrl": ".",
      "paths": {
        "@/*": ["./src/*"]
      }
    }
    // ...
  }
  ```

  - Add the following code to the _vite.config.ts_ so your app can resolve paths without error (so you can import "path" without error)
  - `npm i -D @types/node`

  ```
  import path from "path"
  import react from "@vitejs/plugin-react"
  import { defineConfig } from "vite"
  
  export default defineConfig({
    plugins: [react()],
    resolve: {
      alias: {
        "@": path.resolve(__dirname, "./src"),
      },
    },
  })
  ```

  - `npx shadcn@latest init`
    - Style: New York.
    - Base color: Zinc.
    - CSS variables: yes.

  - That's it! Now install your common components:
    - `npx shadcn@latest add toast`
    - `npx shadcn@latest add input`
    - `npx shadcn@latest add form` (this includes also _button_ and _label_)
    - `npx shadcn@latest add dropdown-menu`
    - `npx shadcn@latest add dialog`
    - `npx shadcn@latest add card`
    - `npx shadcn@latest add table`

- Other useful libs to add:
  - `npm i react-router-dom`
  - `npm i axios`
  - `npm i @tanstack/react-query`
    - `npm i -D @tanstack/eslint-plugin-query` (recommended)
  - `npm i react-hook-form @hookform/resolvers zod`
  - `npm i sweetalert2`
  - `npm i lucide-react`
  
### Once the React project is created, on the root of this repository:
- `npm init`
- In _package.json_, _scripts_, add:
  - _"start": "concurrently \"dotnet watch run\" \"cd client-app && npm run dev\""_
- `npm install --sav-dev concurrently`
- `npm install react react-dom next --sav-dev concurrently`

Now, to run together, just type `npm install` and both backend and frontend are going to start together.
