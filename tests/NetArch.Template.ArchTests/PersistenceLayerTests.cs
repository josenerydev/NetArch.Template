using ArchUnitNET.Fluent;
using ArchUnitNET.xUnit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace NetArch.Template.ArchTests
{
    /// <summary>
    /// Testes que verificam as regras de dependência das camadas de persistência
    /// </summary>
    public class PersistenceLayerTests : BaseArchitectureTest
    {
        /// <summary>
        /// Verifica se a camada Persistence.EntityFrameworkCore depende apenas de Domain e Domain.Shared.
        /// </summary>
        [Fact]
        public void Persistence_EfCore_Should_Only_Depend_On_Domain_Layers()
        {
            // Persistence.EntityFrameworkCore deve depender apenas de si mesma, Domain, Domain.Shared e bibliotecas padrão
            IArchRule rule = Types()
                .That()
                .ResideInNamespace(PersistenceEfCoreNamespace, true)
                .Should()
                .OnlyDependOn(
                    Types()
                        .That()
                        .ResideInNamespace(PersistenceEfCoreNamespace, true)
                        .Or()
                        .ResideInNamespace(DomainNamespace, true)
                        .Or()
                        .ResideInNamespace(DomainSharedNamespace, true)
                        .Or()
                        .ResideInNamespace("System", true)
                        .Or()
                        .ResideInNamespace("Microsoft", true)
                )
                .Because(
                    "Persistence.EntityFrameworkCore deve depender apenas de Domain e Domain.Shared"
                );

            rule.Check(Architecture);
        }

        /// <summary>
        /// Verifica se a camada Persistence.DataAccess depende apenas de Domain e Domain.Shared.
        /// </summary>
        [Fact]
        public void Persistence_DataAccess_Should_Only_Depend_On_Domain_Layers()
        {
            // Persistence.DataAccess deve depender apenas de si mesma, Domain, Domain.Shared e bibliotecas padrão
            IArchRule rule = Types()
                .That()
                .ResideInNamespace(PersistenceDataAccessNamespace, true)
                .Should()
                .OnlyDependOn(
                    Types()
                        .That()
                        .ResideInNamespace(PersistenceDataAccessNamespace, true)
                        .Or()
                        .ResideInNamespace(DomainNamespace, true)
                        .Or()
                        .ResideInNamespace(DomainSharedNamespace, true)
                        .Or()
                        .ResideInNamespace("System", true)
                        .Or()
                        .ResideInNamespace("Microsoft", true)
                )
                .Because("Persistence.DataAccess deve depender apenas de Domain e Domain.Shared");

            rule.Check(Architecture);
        }

        /// <summary>
        /// Verifica se repositórios são implementados apenas nas camadas de persistência.
        /// </summary>
        [Fact]
        public void Repositories_Should_Only_Be_Implemented_In_Persistence_Layers()
        {
            // Para cada interface de repositório no domínio
            var repositoryInterfaces = Interfaces()
                .That()
                .ResideInNamespace(DomainNamespace, true)
                .And()
                .HaveNameEndingWith("Repository");

            // Garanta que todas as classes que contêm 'Repository' no nome estão nas camadas corretas
            IArchRule repositoriesOnlyInPersistenceRule = Classes()
                .That()
                .HaveNameContaining("Repository")
                .And()
                .AreNot(Interfaces())
                .Should()
                .ResideInNamespace(PersistenceEfCoreNamespace, true)
                .OrShould()
                .ResideInNamespace(PersistenceDataAccessNamespace, true)
                .Because(
                    "Implementações de repositórios devem existir apenas nas camadas de persistência"
                );

            repositoriesOnlyInPersistenceRule.Check(Architecture);
        }

        /// <summary>
        /// Verifica se as interfaces de repositório seguem convenções de nomenclatura adequadas.
        /// As interfaces de repositório devem começar com 'I' e terminar com 'Repository'
        /// para indicar claramente seu papel na arquitetura.
        /// </summary>
        [Fact]
        public void Repository_Interfaces_Should_Follow_Naming_Convention()
        {
            IArchRule repositoryNamingRule = Interfaces()
                .That()
                .ResideInNamespace(DomainNamespace, true)
                .And()
                .HaveNameContaining("Repository")
                .Should()
                .HaveNameStartingWith("I")
                .AndShould()
                .HaveNameEndingWith("Repository")
                .Because(
                    "Interfaces de repositório devem começar com 'I' e terminar com 'Repository'"
                );

            repositoryNamingRule.Check(Architecture);
        }
    }
}
