### **ADR-001: Estrutura de Camadas do Projeto**
#### **Status**: Aceito  
#### **Data**: 28/03/2025  
#### **Autor**: Arquitetura de Software  
#### **Última atualização**: 28/03/2025

---

## **Contexto**
Este ADR tem como objetivo definir a estrutura de camadas do projeto **NetArch.Template**, organizando a separação de responsabilidades para garantir modularidade, manutenibilidade e escalabilidade.

O projeto segue princípios de **Clean Architecture**, **Domain-Driven Design (DDD)** e **SOLID**, buscando desacoplamento entre a camada de aplicação, domínio, infraestrutura, apresentação e hosts especializados.

---

## **Decisão**
Foi definida a seguinte estrutura de camadas para o projeto:

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
│   ├── NetArch.Template.IntegrationTests/
│   └── NetArch.Template.E2ETests/
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
- `Infrastructure.Abstractions`: depende de `Application.Contracts`.
- `Infrastructure`: depende de `Infrastructure.Abstractions`, `Domain`, `Application.Contracts`, `Domain.Shared`.
- `Presentation` (HttpApi, Grpc): depende de `Application`, `Application.Contracts`.
- `Hosts`: depende de `Presentation`, `Application`, `Infrastructure`.
- `Tools`: pode depender de `Persistence`, `Infrastructure`, `Application`, conforme necessidade.
- `Tests`: dependem das respectivas camadas testadas e de `TestBase`.

---

## **Consequências**

✅ Estrutura organizada, modular e escalável.  
✅ Suporte a diferentes canais de comunicação.  
✅ Permite crescimento com múltiplos hosts e interfaces.  
✅ Testabilidade e manutenção facilitadas.  
⚠ Requer disciplina para manter dependências corretas e isolamento entre camadas.

---

## **Decisão Final**

A estrutura modular apresentada foi **aceita** como padrão para o projeto **NetArch.Template**, sendo recomendada para projetos corporativos que precisem de flexibilidade, expansão futura, testes robustos e separação clara de responsabilidades.
