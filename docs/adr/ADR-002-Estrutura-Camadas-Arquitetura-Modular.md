### **ADR-002: Estrutura de Camadas da Arquitetura Modular**
#### **Status**: Aceito  
#### **Data**: 28/03/2025  
#### **Autor**: Arquitetura de Software  
#### **Última atualização**: 28/03/2025  

---

## **Contexto**

Este ADR documenta a decisão arquitetural que define a estrutura de camadas da solução **NetArch.Template**, com o objetivo de estabelecer uma arquitetura modular, escalável, testável e alinhada com boas práticas corporativas.

A arquitetura foi desenhada com base nos princípios da **Clean Architecture**, **Domain-Driven Design (DDD)**, **CQRS** e **SOLID**, promovendo separação clara de responsabilidades entre domínio, aplicação, infraestrutura, persistência, canais de apresentação e ambientes de execução.

Essa estrutura permite evolução segura, desacoplamento técnico, substituição de implementações, e facilita a criação de múltiplos pontos de entrada (APIs, workers, serviços gRPC) reutilizando a lógica de negócio central de forma consistente.

---

## **Decisão**

Foi adotada a seguinte estrutura:

```
NetArch.Template/
├── src/
│   ├── Core/
│   │   ├── NetArch.Template.Domain.Shared/
│   │   ├── NetArch.Template.Domain/
│   │   ├── NetArch.Template.Application.Contracts/
│   │   └── NetArch.Template.Application/
│
│   ├── Infrastructure/
│   │   ├── NetArch.Template.Infrastructure.Abstractions/
│   │   └── NetArch.Template.Infrastructure/
│
│   ├── Persistence/
│   │   ├── NetArch.Template.Persistence.EntityFrameworkCore/
│   │   └── NetArch.Template.Persistence.DataAccess/
│
│   ├── Presentation/
│   │   ├── NetArch.Template.HttpApi/
│   │   └── NetArch.Template.Grpc/
│
│   ├── Hosts/
│   │   ├── NetArch.Template.HttpApi.Public/
│   │   ├── NetArch.Template.Worker.Processor/
│   │   └── NetArch.Template.Grpc.Host/
│
│   └── Tools/
│       └── NetArch.Template.DbMigrator/
│
├── tests/
│   ├── NetArch.Template.TestBase/
│   ├── NetArch.Template.UnitTests/
│   └── NetArch.Template.IntegrationTests/
```

---

## **Justificativa**

A estrutura foi definida considerando os seguintes princípios:

1. **Modularidade**: Cada camada possui responsabilidade única e bem definida, evitando sobreposição de funções.  
2. **Baixo Acoplamento**: A separação entre Domínio, Aplicação, Infraestrutura e Hosts garante independência e flexibilidade.  
3. **Facilidade de Testes**: A definição clara de interfaces permite substituir dependências reais por mocks.  
4. **Expansão Horizontal**: A camada de apresentação aceita múltiplos canais (REST, gRPC), cada um com seu host.  
5. **Escalabilidade**: Suporte nativo a APIs, Workers, Processadores e ferramentas.

---

## **Dependências entre Projetos (Resumo)**

- `Domain.Shared`: base comum. Não depende de nada.  
- `Domain`: depende de `Domain.Shared`.  
- `Application.Contracts`: depende de `Domain.Shared`.  
- `Application`: depende de `Domain`, `Domain.Shared`, `Application.Contracts`.  
- `Infrastructure.Abstractions`: depende de `Domain.Shared`.  
- `Infrastructure`: depende de `Infrastructure.Abstractions`, `Domain`, `Domain.Shared`, `Application.Contracts`.  
- `Persistence.EntityFrameworkCore`: depende de `Domain`, `Domain.Shared`.  
- `Persistence.DataAccess`: depende de `Domain`, `Domain.Shared`.  
- `Presentation` (HttpApi, Grpc): depende de `Application`, `Application.Contracts`.  
- `Hosts`: depende de `Presentation`, `Infrastructure`, `Persistence`.  
- `Tools`: pode depender de `Persistence`, `Infrastructure`, `Application`, conforme necessidade.  
- `Tests`: dependem das respectivas camadas testadas e de `TestBase`.

---

## **Descrição das Camadas**

### **Core**

- **Domain.Shared**: Enums, ValueObjects, exceptions e constantes. Reutilizado por toda a solução.  
- **Domain**: Contém entidades, agregados, interfaces de repositório e serviços de domínio. Também define interfaces de queries e serviços que representam regras de negócio, mesmo que sejam implementadas por procedures.  
- **Application.Contracts**: Define as interfaces dos casos de uso, DTOs e contratos públicos da aplicação.  
- **Application**: Implementação dos casos de uso, serviços de orquestração e regras de aplicação.

### **Infrastructure**

- **Infrastructure.Abstractions**: Interfaces técnicas (ex: cache, mensageria, adapters).  
- **Infrastructure**: Implementações concretas das interfaces técnicas, como Redis, RabbitMQ, Stripe, etc.

### **Persistence**

- **Persistence.EntityFrameworkCore**: Implementação de repositórios e `DbContext` usando EF Core.  
- **Persistence.DataAccess**: Acesso a procedures, views e queries diretas. Contém DTOs de banco, implementações de queries e serviços baseados em banco de dados.

### **Presentation**

- **HttpApi**: Controllers REST, filtros, middlewares, validadores e métodos de extensão para registrar serviços da aplicação.  
- **Grpc**: Serviços gRPC, interceptadores e definição de endpoints baseados em `.proto`.

### **Hosts**

- **HttpApi.Public**: Entrypoint REST público. Configura o pipeline do ASP.NET Core e chama os métodos de extensão da camada `Presentation`.  
- **Worker.Processor**: Worker Service para consumidores de fila, cron jobs e tarefas em background.  
- **Grpc.Host**: Host para endpoints gRPC, com configurações específicas e bindings.

### **Tools**

- **DbMigrator**: Ferramenta para execução de migrações e scripts de banco de dados em linha de comando.

### **Tests**

- **TestBase**: Builders, fixtures e mocks reutilizáveis.  
- **UnitTests**: Testes de unidade das camadas Domain e Application.  
- **IntegrationTests**: Testes de integração com banco real, APIs ou serviços externos simulados.

---

## **Decisão Final**

A estrutura com separação de responsabilidades, múltiplos Hosts, múltiplas Presentations e camada de Persistência dedicada foi **aceita** como padrão da arquitetura do projeto **NetArch.Template**, promovendo desacoplamento, expansão segura e clareza organizacional.
