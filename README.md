# ASP.NET Core Hexagonal API

Uma API RESTful desenvolvida com ASP.NET Core 8.0 utilizando arquitetura hexagonal baseada em features.

## 🏗️ Arquitetura

O projeto implementa uma arquitetura hexagonal (Clean Architecture) organizada por features, onde cada feature contém sua própria estrutura de camadas:

```
aspnetcore-hexagonal-api/
├── /Features
│   └── /FeatureExample
│       ├── /Adapters          # Repositórios e acesso a dados
│       ├── /Application       # Serviços de aplicação e constantes
│       ├── /Controllers       # Controllers da API
│       ├── /Domain
│       │   ├── /Enums         # Enumerations do domínio
│       │   ├── /Interfaces    # Interfaces de serviços de domínio
│       │   ├── /Models        # Entidades do domínio
│       │   └── /Validators    # Validadores do FluentValidation
│       ├── /Interfaces        # Contratos de serviços
│       ├── /Models           # DTOs (Request/Response)
│       ├── /Resources        # Arquivos de tradução (.resx)
│       └── /Services         # Regras de negócio
├── /Migrations               # EF Core migrations
├── /Shared                   # Código reutilizável
│   ├── /Extensions          # Extension methods
│   ├── /Helpers            # Classes helper
│   └── /Utilities          # Utilitários
├── ApplicationDbContext.cs   # Contexto do Entity Framework
├── appsettings.json         # Configurações da aplicação
└── Program.cs              # Configuração da aplicação
```

## 🚀 Tecnologias Utilizadas

- **ASP.NET Core 8.0** - Framework web
- **Entity Framework Core 8.0** - ORM para acesso a dados
- **SQL Server** - Banco de dados
- **FluentValidation** - Validação de dados
- **Swagger/OpenAPI** - Documentação da API
- **Serilog** - Logging estruturado
- **Health Checks** - Monitoramento da aplicação

## 📋 Pré-requisitos

- .NET 8.0 SDK
- SQL Server LocalDB ou SQL Server
- Visual Studio 2022 ou VS Code

## 🛠️ Configuração do Projeto

### 1. Clone o repositório
```bash
git clone <repository-url>
cd aspnetcore-hexagonal-api
```

### 2. Restaurar pacotes
```bash
dotnet restore
```

### 3. Configurar banco de dados
```bash
# Criar migration inicial (já existe)
dotnet ef migrations add InitialCreate

# Aplicar migrations
dotnet ef database update
```

### 4. Executar a aplicação
```bash
dotnet run
```

A aplicação estará disponível em:
- **API**: http://localhost:5044
- **Swagger UI**: http://localhost:5044 (página inicial)
- **Health Check**: http://localhost:5044/health

## 🔧 Configuração

### Connection String
Configure a connection string no `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=HexagonalApiDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### Logging
O projeto usa Serilog com saída para console e arquivo:
- Logs são salvos na pasta `logs/`
- Configuração no `appsettings.json`

## 📊 Funcionalidades da API

### Feature Example
Demonstra um CRUD completo com todas as camadas da arquitetura:

**Endpoints disponíveis:**

- `GET /api/FeatureExample` - Lista todos os registros (paginado)
- `GET /api/FeatureExample/{id}` - Busca por ID
- `POST /api/FeatureExample` - Criar novo registro
- `PUT /api/FeatureExample/{id}` - Atualizar registro
- `DELETE /api/FeatureExample/{id}` - Excluir registro (soft delete)

**Parâmetros de consulta:**
- `name` - Filtro por nome
- `status` - Filtro por status
- `createdFrom` / `createdTo` - Filtro por data de criação
- `page` / `pageSize` - Paginação

## 🏛️ Padrões de Design Implementados

### 1. Hexagonal Architecture (Ports & Adapters)
- **Domain**: Entidades e regras de negócio puras
- **Application**: Casos de uso e coordenação
- **Infrastructure**: Implementações técnicas (BD, APIs externas)
- **Presentation**: Controllers e DTOs

### 2. Repository Pattern
```csharp
public interface IFeatureExampleRepository
{
    Task<FeatureExampleEntity?> GetByIdAsync(int id);
    Task<FeatureExampleEntity> CreateAsync(FeatureExampleEntity entity);
    // ... outros métodos
}
```

### 3. Service Layer
- **Domain Services**: Regras de negócio complexas
- **Application Services**: Coordenação e transformação de dados

### 4. DTO Pattern
- Separação entre modelos de domínio e modelos de API
- Request/Response específicos para cada operação

### 5. Validation Pattern
- FluentValidation para validação de entrada
- Validações de negócio nos Domain Services

## 🧪 Testes

### Health Checks
```bash
curl http://localhost:5044/health
# Resposta: Healthy
```

### Teste de CRUD
```bash
# Criar
curl -X POST http://localhost:5044/api/FeatureExample \
  -H "Content-Type: application/json" \
  -d '{"name": "Teste", "description": "Descrição do teste", "status": 1}'

# Listar
curl http://localhost:5044/api/FeatureExample

# Buscar por ID
curl http://localhost:5044/api/FeatureExample/1
```

## 📝 Logs

Os logs são estruturados e incluem:
- Requisições HTTP
- Operações de banco de dados
- Erros e exceções
- Informações de performance

Exemplo de log:
```
[16:05:30 INF] ASP.NET Core Hexagonal API started successfully
[16:05:30 INF] Now listening on: http://localhost:5044
```

## 🔒 Segurança

- Validação de entrada em todos os endpoints
- Soft delete para preservar dados
- Logs estruturados para auditoria
- Health checks para monitoramento

## 🎯 Extensibilidade

### Adicionar nova Feature:
1. Criar estrutura de pastas em `/Features/NovaFeature`
2. Implementar camadas seguindo o padrão existente
3. Registrar dependências em `ServiceCollectionExtensions`
4. Criar migrations se necessário

### Helpers disponíveis:
- `ValidationHelper` - Validação de CPF, CNPJ, Email, etc.
- `StringExtensions` - Manipulação de strings
- `DateTimeHelper` - Operações com datas

## 📞 Suporte

Para suporte e dúvidas:
- Criar uma issue no repositório
- Email: andrefoggiatto10@gmail.com

---

**Desenvolvido usando ASP.NET Core e Arquitetura Hexagonal**