# 📖 Registro de Decisões Arquiteturais (ADRs)

## 📌 Introdução
Este diretório contém os **Arquitectural Decision Records (ADRs)** do projeto **MeuProjeto**.
ADRs são documentos que registram as principais decisões arquiteturais tomadas durante o desenvolvimento do sistema.

O objetivo é garantir **rastreabilidade, transparência e consistência** nas escolhas feitas ao longo do tempo.

---

## 📂 Estrutura de Armazenamento
Os ADRs devem ser armazenados no seguinte diretório dentro do repositório:

```
MeuProjeto/
├── docs/
│   ├── adr/
│   │   ├── ADR-001-Estrutura-Camadas.md
│   │   ├── ADR-002-Padrao-Eventos-Domain-Integration.md
│   │   ├── ADR-003-Escolha-Entre-Dapper-e-EFCore.md
```

Cada ADR é um arquivo Markdown (`.md`), e todos os ADRs são versionados junto ao repositório do código-fonte.

---

## 📛 Nomenclatura dos ADRs
Os ADRs seguem um padrão de nomeação para facilitar a organização e a leitura:

```
ADR-<número sequencial>-<descricao-curta>.md
```

### 📌 Exemplos de ADRs:
- `ADR-001-Estrutura-Camadas.md` → Definição da arquitetura de camadas.
- `ADR-002-Padrao-Eventos-Domain-Integration.md` → Escolha entre Event Sourcing e Domain Events.
- `ADR-003-Escolha-Entre-Dapper-e-EFCore.md` → Decisão sobre ORM e acesso a dados.

---

## 📖 Padrão de Escrita dos ADRs
Cada ADR deve seguir a estrutura padronizada abaixo:

### **1️⃣ Exemplo de Template**

```md
# ADR-XXX: [Título da Decisão]

## 📌 Status
(Aceito, Proposto, Rejeitado, Obsoleto)

## 📅 Data
(DD/MM/AAAA)

## 👥 Autor
(Time de Arquitetura / Nome do Autor)

## 📖 Contexto
(Explique o problema ou necessidade que levou à tomada da decisão.)

## 🏗️ Decisão
(Descreva a solução escolhida e sua implementação.)

## 🎯 Justificativa
(Por que essa solução foi escolhida? Cite benefícios e motivação.)

## 🔄 Alternativas Consideradas
(Descreva opções analisadas e por que foram descartadas.)

## ⚠️ Consequências
(Explique impactos positivos e negativos da decisão.)

## ✅ Decisão Final
(A confirmação de que essa é a solução adotada.)
```

---

## 🔄 Atualizações de ADRs
Uma vez que um ADR é aceito, ele **não deve ser modificado**. Se uma decisão mudar no futuro, deve ser criado um **novo ADR**, referenciando o ADR original e explicando a nova direção tomada.

Exemplo:
```
ADR-010 substitui ADR-002 devido à mudança na estratégia de mensageria.
```

---

## 🚀 Benefícios do Uso de ADRs
✅ **Histórico de decisões** → Mantém um registro claro das escolhas feitas.  
✅ **Facilidade para novos devs** → Documentação para onboardings mais rápidos.  
✅ **Rastreabilidade** → Explica o porquê das decisões técnicas.  
✅ **Governança** → Evita discussões repetitivas sobre a arquitetura.  
✅ **Facilidade de mudança** → Permite revisão estruturada de decisões obsoletas.  

---

## 📌 Conclusão
Este repositório de ADRs deve ser **mantido e atualizado** ao longo do tempo para documentar as principais decisões arquiteturais do projeto.
Se houver novas mudanças ou alterações significativas na arquitetura, um novo ADR deve ser criado para documentar a decisão.

Se precisar sugerir uma nova decisão arquitetural, siga o modelo e crie um novo ADR! 🚀
