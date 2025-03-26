### **ADR-001: Estrutura de Camadas do Projeto**
#### **Status**: Aceito  
#### **Data**: 26/03/2025  
#### **Autor**: Arquitetura de Software  
#### **Última atualização**: 26/03/2025

---

## **Contexto**
Este ADR tem como objetivo definir a estrutura de camadas do projeto **MeuProjeto**, organizando a separação de responsabilidades para garantir modularidade, manutenibilidade e escalabilidade.

O projeto segue princípios de **Clean Architecture**, **Domain-Driven Design (DDD)** e **SOLID**, buscando desacoplamento entre a camada de aplicação, domínio, infraestrutura e apresentação.

---

## **Decisão**
Foi definida a seguinte estrutura de camadas para o projeto:

MeuProjeto
 ├── src/
 │   ├── Core/
 │   │   ├── MeuProjeto.Domain.Shared/
 │   │   │   ├── DomainConstants.cs
 │   │   ├── MeuProjeto.Domain/
 │   │   │   ├── Entities/
 │   │   │   │   ├── Order.cs
 │   │   │   ├── Services/
 │   │   │   │   ├── OrderDomainService.cs
 │   │   │   │   ├── IOrderProcessingService.cs
 │   │   │   ├── Repositories/
 │   │   │   │   ├── IOrderRepository.cs
 │   │   │   ├── ReadModels/
 │   │   │   │   ├── IOrderSummaryQuery.cs
 │   │   ├── MeuProjeto.Application.Contracts/
 │   │   │   ├── DTOs/
 │   │   │   │   ├── OrderDto.cs
 │   │   │   ├── Services/
 │   │   │   │   ├── IOrderAppService.cs
 │   │   │   ├── UseCases/
 │   │   │   │   ├── IProcessOrderUseCase.cs
 │   │   │   ├── Workflows/
 │   │   │   │   ├── IOrderProcessingWorkflow.cs
 │   │   ├── MeuProjeto.Application/
 │   │   │   ├── Services/
 │   │   │   │   ├── OrderAppService.cs
 │   │   │   ├── UseCases/
 │   │   │   │   ├── ProcessOrderUseCase.cs
 │   │   │   ├── Workflows/
 │   │   │   │   ├── OrderProcessingWorkflow.cs
 │   ├── Infrastructure/
 │   │   ├── MeuProjeto.Infrastructure.Abstractions/
 │   │   │   ├── Caching/
 │   │   │   │   ├── ICacheService.cs
 │   │   │   ├── Messaging/
 │   │   │   │   ├── IMessageBus.cs
 │   │   │   ├── Adapters/
 │   │   │   │   ├── IPaymentAdapter.cs
 │   │   ├── MeuProjeto.Infrastructure/
 │   │   │   ├── Caching/
 │   │   │   │   ├── RedisCacheService.cs
 │   │   │   ├── Bus/
 │   │   │   │   ├── RabbitMqBusService.cs
 │   │   │   ├── Adapters/
 │   │   │   │   ├── StripePaymentAdapter.cs
 │   ├── Persistence/
 │   │   ├── MeuProjeto.Persistence.EntityFrameworkCore/
 │   │   │   ├── AppDbContext.cs
 │   │   │   ├── OrderRepository.cs
 │   │   ├── MeuProjeto.Persistence.DataAccess/
 │   │   │   ├── IUnitOfWork.cs
 │   │   │   ├── GenericRepository.cs
 │   │   │   ├── Services/
 │   │   │   │   ├── OrderProcessingService.cs
 │   │   │   ├── Queries/
 │   │   │   │   ├── OrderSummaryQuery.cs
 │   │   │   ├── Scripts/
 │   │   │   │   ├── CreateProcedures.sql
 │   │   │   │   ├── CreateViews.sql
 │   ├── CrossCutting/
 │   │   ├── MeuProjeto.CrossCutting.Logging/
 │   │   │   ├── SerilogConfig.cs
 │   │   ├── MeuProjeto.CrossCutting.Security/
 │   │   │   ├── JwtService.cs
 │   ├── Presentation/
 │   │   ├── MeuProjeto.HttpApi/
 │   │   │   ├── Controllers/
 │   │   │   │   ├── OrderController.cs
 │   ├── Hosts/
 │   │   ├── MeuProjeto.HttpApi.Public/
 │   │   │   ├── Program.cs
 │   │   ├── MeuProjeto.HttpApi.Internal/
 │   │   │   ├── Program.cs
 │   │   ├── MeuProjeto.Worker/
 │   │   │   ├── WorkerService.cs
 │   ├── Tools/
 │   │   ├── MeuProjeto.DbMigrator/
 │   │   │   ├── MigrationRunner.cs
 ├── tests/
 │   ├── MeuProjeto.Core.Tests/
 │   │   ├── OrderTests.cs
 │   ├── MeuProjeto.Application.Tests/
 │   │   ├── OrderAppServiceTests.cs
 │   │   ├── ProcessOrderUseCaseTests.cs
 │   │   ├── OrderProcessingWorkflowTests.cs
 │   ├── MeuProjeto.Persistence.Tests/
 │   │   ├── OrderRepositoryTests.cs
 │   │   ├── OrderProceduresTests.cs
 │   │   ├── OrderViewsTests.cs


---

### **Justificativa**  
A estrutura foi definida considerando os seguintes princípios:  

1. **Modularidade**: Cada camada possui responsabilidade única e bem definida, evitando sobreposição de funções.  
2. **Baixo Acoplamento**: A separação entre **Domínio, Aplicação, Infraestrutura e Persistência** garante independência e flexibilidade.  
3. **Facilidade de Testes**: A definição clara de interfaces permite substituir dependências reais por mocks durante os testes.  
4. **Escalabilidade**: A modularização possibilita expansão sem grandes refatorações, permitindo o crescimento sustentável do projeto.  
5. **Facilidade de Manutenção**: A organização intuitiva do código simplifica a manutenção e a adoção por novos desenvolvedores.  

---

### **Relacionamento entre as Camadas**  

| Camada | Descrição | Dependências |
|--------|----------|-------------|
| **Core** | Define a lógica de negócio e aplicação (Domínio, Casos de Uso e Serviços). | Nenhuma |
| **Core/Domain.Shared** | Contém contratos e definições compartilhadas dentro do domínio. | Core/Domain |
| **Core/Domain** | Define as entidades, repositórios e regras de negócio. | Core/Domain.Shared |
| **Core/Domain/Services** | Implementa regras de negócio puras sem dependências externas. | Core/Domain |
| **Core/Application.Contracts** | Define interfaces para Serviços, Use Cases e Workflows. | Core/Domain |
| **Core/Application** | Implementa a lógica de aplicação, incluindo Serviços, Use Cases e Workflows. | Core/Application.Contracts, Core/Domain |
| **Infrastructure.Abstractions** | Define contratos técnicos para infraestrutura, como Cache, Mensageria e Adapters. | Nenhuma |
| **Infrastructure** | Implementa serviços de infraestrutura como Redis, RabbitMQ e Adapters de pagamento. | Infrastructure.Abstractions |
| **Persistence/EntityFrameworkCore** | Implementa a persistência específica para Entity Framework Core. | Core/Domain |
| **Persistence/DataAccess** | Contém todas as implementações de acesso a dados que não são específicas do EF Core, incluindo executores de stored procedures, leitores de views, outros mecanismos de acesso a dados, e scripts SQL. | Core/Domain |
| **CrossCutting** | Gerencia aspectos transversais como logging, segurança e validação. | Nenhuma |
| **Presentation** | Define a API HTTP, controladores REST e interfaces de entrada. | Core/Application |
| **Hosts** | Contém as configurações de serviços como APIs públicas, APIs internas e Workers. | Presentation, Infrastructure |
| **Tools** | Contém ferramentas auxiliares, como scripts de migração de banco de dados. | Persistence |
| **Tests** | Contém testes unitários e de integração. | Dependências das camadas testadas |
| **CrossCutting** | Gerencia aspectos transversais como logging, segurança e validação. | Nenhuma |
| **Presentation** | Define a API HTTP, controladores REST e interfaces de entrada. | Core/Application |
| **Hosts** | Contém as configurações de serviços como APIs públicas, APIs internas e Workers. | Presentation, Infrastructure |
| **Tools** | Contém ferramentas auxiliares, como scripts de migração de banco de dados. | Persistence |
| **Tests** | Contém testes unitários e de integração. | Dependências das camadas testadas |

---

### **Observações sobre Procedures e Views com Regras de Negócio**

Uma consideração importante nesta arquitetura é o tratamento de procedures e views que contêm regras de negócio:

1. **Interfaces e Abstrações**:
   - Para procedures que executam operações de negócio:
     - As interfaces são definidas como serviços em `MeuProjeto.Domain/Services`.
     - Exemplo: `IOrderProcessingService.cs` 
   - Para views que fornecem dados agregados ou relatórios:
     - As interfaces são definidas como queries em `MeuProjeto.Domain/ReadModels`.
     - Exemplo: `IOrderSummaryQuery.cs`

2. **Separação de Implementações**:
   - Todas as implementações específicas do Entity Framework Core ficam em `MeuProjeto.Persistence.EntityFrameworkCore`.
   - Todas as demais implementações de acesso a dados ficam em `MeuProjeto.Persistence.DataAccess`, incluindo:
     - Implementações de serviços para procedures
     - Implementações de queries para views
     - Adapters para outros mecanismos de persistência (NoSQL, APIs externas, etc.)
     - Implementações de repositórios não-EF
     - Scripts SQL para criação de procedures e views

3. **Scripts SQL**:
   - Os scripts SQL são armazenados no projeto `MeuProjeto.Persistence.DataAccess/Scripts`.
   - Esta organização mantém todo o código relacionado a acesso a dados em um único projeto.

4. **Regras de Negócio**:
   - Embora procedures possam conter regras de negócio, a camada de domínio permanece como fonte primária de regras.
   - Procedures devem ser usadas principalmente para:
     - Operações em lote que necessitam de alto desempenho
     - Processos transacionais complexos
     - Queries de relatórios otimizadas
   - A interface no domínio define claramente o contrato de negócio, independente da implementação.

Esse design segue princípios de CQRS (Command Query Responsibility Segregation):
- **Commands** (Serviços): Operações que modificam dados
- **Queries**: Componentes que recuperam visualizações especializadas de dados

Esta abordagem permite que:
- As interfaces fiquem no domínio, mantendo o controle da regra de negócio nesta camada
- As implementações fiquem organizadas por tecnologia (EF Core vs. outros mecanismos)
- O domínio continue puro (sem dependências de infraestrutura)
- As implementações de banco de dados possam ser substituídas sem afetar o domínio

---

### **Consequências**  

✅ **Benefícios**  
- Estrutura bem organizada e modular.  
- Maior facilidade para testes unitários e integração.  
- Possibilidade de substituir tecnologias sem impactar o núcleo do sistema.  
- Escalabilidade para futuras evoluções do projeto.  
- Encapsulamento adequado de procedures e views com regras de negócio.

⚠ **Possíveis Desafios**  
- Pode parecer complexa para novos desenvolvedores que não estejam acostumados com arquitetura modular.  
- Requer governança para manter a separação correta entre camadas e evitar violações de dependência.  
- Necessidade de sincronizar regras de negócio entre o domínio e procedures de banco de dados.

---

### **Alternativas Consideradas**  

1. **Monólito Simples**: Toda a lógica misturada em um único projeto. **(Rejeitado - dificulta manutenção e escalabilidade)**  
2. **Arquitetura Hexagonal (Ports & Adapters)**: Desacopla completamente as interações externas. **(Aceitável, mas adiciona complexidade desnecessária ao escopo atual)**  
3. **Arquitetura em Camadas Clássica**: Separa Domínio, Aplicação e Infraestrutura, mas sem modularização explícita. **(Menos flexível para mudanças a longo prazo)**  
4. **Embutir Procedures no Domínio**: Implementar procedures e views diretamente como serviços de domínio. **(Rejeitado - violaria a pureza do domínio)**

A arquitetura escolhida proporciona um equilíbrio entre **flexibilidade, organização e facilidade de manutenção**, garantindo que o projeto possa evoluir de forma sustentável.  

---

### **Decisão Final**  
A estrutura modular foi **aceita** e será adotada como padrão para o projeto **MeuProjeto**, incluindo o tratamento adequado para procedures e views com regras de negócio.
