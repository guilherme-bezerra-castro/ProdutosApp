# ProdutosApp

Sistema CRUD de cadastro de produtos, desenvolvido em ASP.NET Core MVC com Entity Framework Core e MySQL, como parte do teste prático para a vaga de Estágio em Desenvolvimento Web.

## Tecnologias

- C# / ASP.NET Core 8 MVC
- Entity Framework Core 8 (Pomelo.EntityFrameworkCore.MySql) + MySQL
- Bootstrap 5
- Autenticação por Cookie (`Microsoft.AspNetCore.Authentication.Cookies`) + hash de senha via `PasswordHasher<TUser>` (`Microsoft.Extensions.Identity.Core`)
- Arquitetura em camadas: Controller → Service → Repository → DbContext

## Funcionalidades

### Obrigatórias
- Cadastrar produto (Descrição, Quantidade, Valor, Usuário, Data de Cadastro)
- Listar todos os produtos
- Ver produto por ID
- Editar produto por ID
- Excluir produto por ID

### Diferenciais implementados
- Ordenação ASC/DESC dinâmica por ID, Data de Cadastro, Usuário e Valor
- Filtros por Descrição e por Usuário na tela de listagem
- Tela de Login e cadastro de usuários
- Preenchimento automático de Usuário e Data de Cadastro a partir do usuário logado
- Proteção das telas de produtos com `[Authorize]` (exige login)

## Como rodar o projeto localmente

### Pré-requisitos
- .NET SDK 8
- MySQL Server

### Passos

1. Clone o repositório:
   ```bash
   git clone https://github.com/SEU_USUARIO/ProdutosApp.git
   cd ProdutosApp
   ```

2. Crie o arquivo `appsettings.Development.json` (não versionado) com sua connection string:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Port=3306;Database=produtosdb;Uid=root;Pwd=SUASENHA;"
     }
   }
   ```

3. Crie o banco rodando o script pronto em `database/schema.sql`, OU aplique as migrations do EF Core:
   ```bash
   dotnet ef database update
   ```

4. Rode o projeto:
   ```bash
   dotnet run
   ```

5. Acesse a URL exibida no terminal (ex: `http://localhost:5145`).

6. Crie uma conta em **Criar conta** e faça login para acessar o CRUD de produtos.

## Estrutura do projeto

```
ProdutosApp/
├── Controllers/
│   ├── ProdutosController.cs   -> CRUD de produtos (protegido por login)
│   ├── AuthController.cs       -> Login, registro e logout
│   └── HomeController.cs
├── Models/
│   ├── Produto.cs
│   ├── Usuario.cs
│   └── Auth/
│       ├── LoginViewModel.cs
│       └── RegistroViewModel.cs
├── Data/
│   └── AppDbContext.cs
├── Repositories/                -> Acesso a dados (EF Core)
├── Services/                    -> Regras de negócio
├── Views/
│   ├── Produtos/                -> Index, Create, Edit, Details, Delete
│   ├── Auth/                    -> Login, Registrar
│   └── Shared/
├── Migrations/                  -> Migrations do EF Core
database/
└── schema.sql                   -> Script de criação das tabelas (Produtos + Usuarios)
```
