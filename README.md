## Teste para vaga de Desenvolvimento Back-end .NET
---

### Especificação 🛠️

- Versão .NET: 8
- Entity Framework
- Aspnetboilerplate
- Docker:
  - Sql Server 2022
- XUnit

- Sugestão:
  - Implementar Job de verificação dos dados da API Billing, Pode ser utilizado o Hangfire que tem suporte junto com Aspnetboilerplate

---

### Configuração 💻

#### Instalação do Docker

Certifique-se de ter o Docker instalado em sua máquina. Você pode baixá-lo e instalá-lo a partir do [site oficial do Docker](https://www.docker.com/get-started).

#### Instalação do SQL Server 2022 via Docker

Utilize o Docker para executar uma instância do SQL Server 2022. Você pode fazer isso executando o seguinte comando no terminal:

```bash
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=sua_senha_aqui" -p 1433:1433 --name sql_server_2022 -d mcr.microsoft.com/mssql/server:2022-latest
```

- `ACCEPT_EULA=Y`: Aceita o contrato de licença.
- `SA_PASSWORD=sua_senha_aqui`: Define a senha do usuário `sa` do SQL Server. Substitua `sua_senha_aqui` pela senha desejada.
- `1433:1433`: Mapeia a porta 1433 do host para a porta 1433 do contêiner Docker (porta padrão do SQL Server).

Certifique-se de que o contêiner está em execução usando `docker ps -a`.

### Configuração do Projeto .NET

1. Clone ou baixe o projeto do repositório.
2. Abra o projeto usando o Visual Studio ou qualquer editor de código de sua preferência que suporte .NET 8 e Entity Framework.
3. Abra um terminal na raiz do projeto onde está localizado o arquivo `.csproj`.
4. Execute o seguinte comando para aplicar as migrações e atualizar o banco de dados:

```bash
   dotnet ef database update
```

Este comando aplica todas as migrações pendentes ao banco de dados configurado no projeto. Certifique-se de que seu arquivo `appsettings.json` (ou `appsettings.Development.json`) esteja configurado corretamente para se conectar ao banco de dados SQL Server.

### Observações

- Certifique-se de que todas as dependências do projeto estejam instaladas e configuradas corretamente, como o Entity Framework e o Aspnetboilerplate.
- Se necessário, ajuste o connection string no arquivo de configuração do projeto para refletir as configurações corretas do SQL Server Docker (nome do servidor, porta, usuário, senha, etc.).

