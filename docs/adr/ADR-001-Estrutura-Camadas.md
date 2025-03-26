### **ADR-001: Estrutura de Camadas do Projeto**
#### **Status**: Aceito  
#### **Data**: 26/03/2025  
#### **Autor**: Arquitetura de Software  

---

## **Contexto**
Este ADR tem como objetivo definir a estrutura de camadas do projeto **MeuProjeto**, organizando a separação de responsabilidades para garantir modularidade, manutenibilidade e escalabilidade.

O projeto segue princípios de **Clean Architecture**, **Domain-Driven Design (DDD)** e **SOLID**, buscando desacoplamento entre a camada de aplicação, domínio, infraestrutura e apresentação.

---

## **Decisão**
Foi definida a seguinte estrutura de camadas para o projeto:

```
MeuProjeto
 ├── src/
 │   ├── MeuProjeto.Shared/
 │   │   ├── Constants.cs
 │   ├── MeuProjeto.Core/
 │   │   ├── Entities/
 │   │   │   ├── Order.cs
 │   │   ├── Services/
 │   │   │   ├── OrderService.cs
 │   │   ├── Repositories/
 │   │   │   ├── IOrderRepository.cs
 │   ├── MeuProjeto.Application.Contracts/
 │   │   ├── DTOs/
 │   │   │   ├── OrderDto.cs
 │   │   ├── Services/
 │   │   │   ├── IOrderAppService.cs
 │   │   ├── UseCases/
 │   │   │   ├── IProcessOrderUseCase.cs
 │   │   ├── Workflows/
 │   │   │   ├── IOrderProcessingWorkflow.cs
 │   ├── MeuProjeto.Application/
 │   │   ├── Services/
 │   │   │   ├── OrderAppService.cs
 │   │   ├── UseCases/
 │   │   │   ├── ProcessOrderUseCase.cs
 │   │   ├── Workflows/
 │   │   │   ├── OrderProcessingWorkflow.cs
 │   ├── MeuProjeto.Infrastructure.Abstractions/
 │   │   ├── Caching/
 │   │   │   ├── ICacheService.cs
 │   │   ├── Messaging/
 │   │   │   ├── IMessageBus.cs
 │   │   ├── Adapters/
 │   │   │   ├── IPaymentAdapter.cs
 │   ├── MeuProjeto.Infrastructure/
 │   │   ├── Caching/
 │   │   │   ├── RedisCacheService.cs
 │   │   ├── Bus/
 │   │   │   ├── RabbitMqBusService.cs
 │   │   ├── Adapters/
 │   │   │   ├── StripePaymentAdapter.cs
 │   ├── Persistence/
 │   │   ├── MeuProjeto.Persistence.EntityFrameworkCore/
 │   │   │   ├── AppDbContext.cs
 │   │   │   ├── OrderRepository.cs
 │   │   ├── MeuProjeto.Persistence.DataAccess/
 │   │   │   ├── IUnitOfWork.cs
 │   │   │   ├── GenericRepository.cs
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
 ├── README.md
 ├── .gitignore
 ├── docker-compose.yml
```

---

## **Justificativa**
A estrutura foi definida considerando os seguintes princípios:

1. **Modularidade**: Cada camada tem responsabilidade única e bem definida.
2. **Baixo Acoplamento**: A separação entre Contracts, Abstractions e Implementações evita dependências diretas.
3. **Facilidade de Testes**: As interfaces permitem substituir dependências reais por mocks.
4. **Escalabilidade**: Permite a expansão da aplicação sem refatorações drásticas.
5. **Facilidade de Manutenção**: O código é organizado e segmentado de forma intuitiva.

---

## **Relacionamento entre as Camadas**

| Camada | Descrição | Dependências |
|--------|----------|-------------|
| **Shared** | Contém constantes e utilitários comuns. | Nenhuma |
| **Core** | Define a lógica de domínio (Entidades, Serviços e Repositórios). | Shared |
| **Application.Contracts** | Define interfaces para Serviços, Use Cases e Workflows. | Core, Shared |
| **Application** | Implementa a lógica de aplicação (Casos de Uso, Serviços e Workflows). | Application.Contracts, Core, Shared |
| **Infrastructure.Abstractions** | Define contratos técnicos para Cache, Mensageria e Adapters. | Shared |
| **Infrastructure** | Implementa serviços de infraestrutura como Redis, RabbitMQ e Stripe. | Infrastructure.Abstractions, Shared |
| **Persistence** | Implementa a camada de persistência com EF Core e DataAccess. | Core, Shared |
| **CrossCutting** | Gerencia logging, segurança e outras preocupações transversais. | Shared |
| **Presentation** | Define a API HTTP e os controladores REST. | Application, Shared |
| **Hosts** | Configura serviços como APIs, Workers e Background Jobs. | Application, Infrastructure, Shared |
| **Tools** | Scripts auxiliares como migração de banco de dados. | Persistence, Shared |
| **Tests** | Testes unitários e de integração. | Dependências das camadas testadas |

---

## **Consequências**
✅ **Benefícios**
- Melhor organização e modularidade.
- Facilidade para testes unitários e de integração.
- Facilita a substituição de tecnologias na infraestrutura.

⚠ **Possíveis Desafios**
- Pode parecer complexo inicialmente devido à separação de camadas.
- Exige boa governança para manter a organização.

---

## **Alternativas Consideradas**
1. **Monólito Simples**: Sem separação de camadas, tudo junto. **(Rejeitado - difícil de escalar e manter)**
2. **Arquitetura Hexagonal (Ports & Adapters)**: Abstrai completamente a infraestrutura. **(Aceitável, mas mais complexo)**
3. **Arquitetura em Camadas Clássica**: Separação básica entre domínio, aplicação e infraestrutura. **(Menos flexível para mudanças)**

A arquitetura escolhida balanceia flexibilidade, organização e facilidade de manutenção.

---

## **Decisão Final**
A estrutura modular foi **aceita** e será adotada como padrão para o projeto **MeuProjeto**.
