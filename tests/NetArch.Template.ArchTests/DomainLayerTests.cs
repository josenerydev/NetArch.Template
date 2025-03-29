using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.xUnit;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace NetArch.Template.ArchTests
{
    /// <summary>
    /// Testes que verificam as regras de dependência das camadas de domínio
    /// </summary>
    public class DomainLayerTests : BaseArchitectureTest
    {
        /// <summary>
        /// Verifica se a camada Domain.Shared não depende de nenhuma outra camada.
        /// </summary>
        [Fact]
        public void Domain_Shared_Should_Not_Depend_On_Any_Layer()
        {
            // Domain.Shared deve depender apenas de si mesmo e bibliotecas padrão
            IArchRule rule = Types()
                .That()
                .ResideInNamespace(DomainSharedNamespace, true)
                .Should()
                .OnlyDependOn(
                    Types()
                        .That()
                        .ResideInNamespace(DomainSharedNamespace, true)
                        .Or()
                        .ResideInNamespace("System", true)
                        .Or()
                        .ResideInNamespace("Microsoft", true)
                )
                .Because("Domain.Shared deve ser totalmente independente das outras camadas");

            rule.Check(Architecture);
        }

        /// <summary>
        /// Verifica se a camada Domain depende apenas de Domain.Shared.
        /// </summary>
        [Fact]
        public void Domain_Should_Only_Depend_On_Domain_Shared()
        {
            // Domain deve depender apenas de si mesmo, Domain.Shared e bibliotecas padrão
            IArchRule rule = Types()
                .That()
                .ResideInNamespace(DomainNamespace, true)
                .Should()
                .OnlyDependOn(
                    Types()
                        .That()
                        .ResideInNamespace(DomainNamespace, true)
                        .Or()
                        .ResideInNamespace(DomainSharedNamespace, true)
                        .Or()
                        .ResideInNamespace("System", true)
                        .Or()
                        .ResideInNamespace("Microsoft", true)
                )
                .Because("Domain deve depender apenas de Domain.Shared");

            rule.Check(Architecture);
        }

        /// <summary>
        /// Verifica se as entidades de domínio não possuem dependências diretas de frameworks de persistência.
        /// </summary>
        [Fact]
        public void Domain_Entities_Should_Not_Have_Persistence_Dependencies()
        {
            IArchRule domainEntitiesNoPersistenceRule = Classes()
                .That()
                .ResideInNamespace(DomainNamespace, true)
                .And()
                .AreNot(Interfaces())
                .And()
                .DoNotHaveNameEndingWith("Factory")
                .And()
                .DoNotHaveNameEndingWith("Service")
                .And()
                .DoNotHaveNameEndingWith("Repository")
                .Should()
                .NotDependOnAny(
                    Types().That().ResideInNamespace("Microsoft.EntityFrameworkCore", true)
                )
                .Because("Entidades de domínio não devem depender de frameworks de persistência");

            domainEntitiesNoPersistenceRule.Check(Architecture);
        }

        /// <summary>
        /// Verifica se as entidades de domínio seguem convenções de nomenclatura adequadas.
        /// As entidades devem ter nomes significativos e não devem terminar com sufixos como 'Entity'.
        /// </summary>
        [Fact]
        public void Domain_Entities_Should_Follow_Naming_Convention()
        {
            IArchRule entitiesNamingRule = Classes()
                .That()
                .ResideInNamespace(DomainNamespace, true)
                .And()
                .AreNot(Interfaces())
                .And()
                .AreNotEnums()
                .And()
                .DoNotHaveNameEndingWith("Factory")
                .And()
                .DoNotHaveNameEndingWith("Service")
                .And()
                .DoNotHaveNameEndingWith("Repository")
                .Should()
                .NotHaveNameEndingWith("Entity")
                .Because("Entidades de domínio não devem ter o sufixo 'Entity'");

            entitiesNamingRule.Check(Architecture);
        }
    }
}
