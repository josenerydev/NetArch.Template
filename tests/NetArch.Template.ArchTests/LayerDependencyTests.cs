using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.xUnit;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace NetArch.Template.ArchTests
{
    /// <summary>
    /// Testes que verificam as regras de dependência entre camadas da arquitetura conforme o ADR
    /// </summary>
    public class LayerDependencyTests : BaseArchitectureTest
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
            // Domain deve depender apenas de Domain.Shared
            IArchRule rule = Types()
                .That()
                .ResideInNamespace(DomainNamespace, true)
                .Should()
                .NotDependOnAny(
                    Types()
                        .That()
                        .ResideInNamespace(ApplicationContractsNamespace, true)
                        .Or()
                        .ResideInNamespace(ApplicationNamespace, true)
                        .Or()
                        .ResideInNamespace(InfrastructureAbstractionsNamespace, true)
                        .Or()
                        .ResideInNamespace(InfrastructureNamespace, true)
                        .Or()
                        .ResideInNamespace(PersistenceEfCoreNamespace, true)
                        .Or()
                        .ResideInNamespace(PersistenceDataAccessNamespace, true)
                        .Or()
                        .ResideInNamespace(HttpApiNamespace, true)
                        .Or()
                        .ResideInNamespace(HttpApiPublicNamespace, true)
                )
                .Because("Domain deve depender apenas de Domain.Shared");

            rule.Check(Architecture);
        }

        /// <summary>
        /// Verifica se a camada Application.Contracts depende apenas de Domain.Shared.
        /// </summary>
        [Fact]
        public void Application_Contracts_Should_Only_Depend_On_Domain_Shared()
        {
            // Verifica se Application.Contracts depende somente de si mesma e Domain.Shared,
            // mas não de outras camadas
            IArchRule rule = Types()
                .That()
                .ResideInNamespace(ApplicationContractsNamespace, true)
                .Should()
                .OnlyDependOn(
                    Types()
                        .That()
                        .ResideInNamespace(ApplicationContractsNamespace, true)
                        .Or()
                        .ResideInNamespace(DomainSharedNamespace, true)
                        .Or()
                        .ResideInNamespace("System", true)
                        .Or()
                        .ResideInNamespace("Microsoft", true)
                )
                .Because("Application.Contracts deve depender apenas de Domain.Shared");

            rule.Check(Architecture);
        }

        /// <summary>
        /// Verifica se a camada Application depende apenas de Domain, Domain.Shared, Application.Contracts
        /// e Infrastructure.Abstractions.
        /// </summary>
        [Fact]
        public void Application_Should_Only_Depend_On_Allowed_Layers()
        {
            // Verifica se Application depende somente de camadas permitidas
            IArchRule rule = Types()
                .That()
                .ResideInNamespace(ApplicationNamespace, true)
                .Should()
                .OnlyDependOn(
                    Types()
                        .That()
                        .ResideInNamespace(ApplicationNamespace, true)
                        .Or()
                        .ResideInNamespace(DomainNamespace, true)
                        .Or()
                        .ResideInNamespace(DomainSharedNamespace, true)
                        .Or()
                        .ResideInNamespace(ApplicationContractsNamespace, true)
                        .Or()
                        .ResideInNamespace(InfrastructureAbstractionsNamespace, true)
                        .Or()
                        .ResideInNamespace("System", true)
                        .Or()
                        .ResideInNamespace("Microsoft", true)
                )
                .Because(
                    "Application deve depender apenas de Domain, Domain.Shared, Application.Contracts e Infrastructure.Abstractions"
                );

            rule.Check(Architecture);
        }

        /// <summary>
        /// Verifica se a camada Infrastructure.Abstractions depende apenas de Domain.Shared.
        /// </summary>
        [Fact]
        public void Infrastructure_Abstractions_Should_Only_Depend_On_Domain_Shared()
        {
            // Infrastructure.Abstractions deve depender apenas de si mesma e Domain.Shared
            IArchRule rule = Types()
                .That()
                .ResideInNamespace(InfrastructureAbstractionsNamespace, true)
                .Should()
                .OnlyDependOn(
                    Types()
                        .That()
                        .ResideInNamespace(InfrastructureAbstractionsNamespace, true)
                        .Or()
                        .ResideInNamespace(DomainSharedNamespace, true)
                        .Or()
                        .ResideInNamespace("System", true)
                        .Or()
                        .ResideInNamespace("Microsoft", true)
                )
                .Because("Infrastructure.Abstractions deve depender apenas de Domain.Shared");

            rule.Check(Architecture);
        }

        /// <summary>
        /// Verifica se a camada Infrastructure depende apenas de camadas permitidas:
        /// Infrastructure.Abstractions, Domain, Domain.Shared e Application.Contracts.
        /// </summary>
        [Fact]
        public void Infrastructure_Should_Only_Depend_On_Allowed_Layers()
        {
            // Infrastructure deve depender apenas de Infrastructure.Abstractions, Domain, Domain.Shared, Application.Contracts
            IArchRule rule = Types()
                .That()
                .ResideInNamespace(InfrastructureNamespace, true)
                .Should()
                .NotDependOnAny(
                    Types()
                        .That()
                        .ResideInNamespace(ApplicationNamespace, true)
                        .Or()
                        .ResideInNamespace(PersistenceEfCoreNamespace, true)
                        .Or()
                        .ResideInNamespace(PersistenceDataAccessNamespace, true)
                        .Or()
                        .ResideInNamespace(HttpApiNamespace, true)
                        .Or()
                        .ResideInNamespace(HttpApiPublicNamespace, true)
                )
                .Because(
                    "Infrastructure deve depender apenas de Infrastructure.Abstractions, Domain, Domain.Shared e Application.Contracts"
                );

            rule.Check(Architecture);
        }

        /// <summary>
        /// Verifica se as camadas de persistência dependem apenas de Domain e Domain.Shared.
        /// </summary>
        [Fact]
        public void Persistence_Layers_Should_Only_Depend_On_Domain_Layers()
        {
            // Persistence.EntityFrameworkCore deve depender apenas de Domain e Domain.Shared
            IArchRule efCoreRule = Types()
                .That()
                .ResideInNamespace(PersistenceEfCoreNamespace, true)
                .Should()
                .NotDependOnAny(
                    Types()
                        .That()
                        .ResideInNamespace(ApplicationContractsNamespace, true)
                        .Or()
                        .ResideInNamespace(ApplicationNamespace, true)
                        .Or()
                        .ResideInNamespace(InfrastructureAbstractionsNamespace, true)
                        .Or()
                        .ResideInNamespace(InfrastructureNamespace, true)
                        .Or()
                        .ResideInNamespace(PersistenceDataAccessNamespace, true)
                        .Or()
                        .ResideInNamespace(HttpApiNamespace, true)
                        .Or()
                        .ResideInNamespace(HttpApiPublicNamespace, true)
                )
                .Because(
                    "Persistence.EntityFrameworkCore deve depender apenas de Domain e Domain.Shared"
                );

            // Persistence.DataAccess deve depender apenas de Domain e Domain.Shared
            IArchRule dataAccessRule = Types()
                .That()
                .ResideInNamespace(PersistenceDataAccessNamespace, true)
                .Should()
                .NotDependOnAny(
                    Types()
                        .That()
                        .ResideInNamespace(ApplicationContractsNamespace, true)
                        .Or()
                        .ResideInNamespace(ApplicationNamespace, true)
                        .Or()
                        .ResideInNamespace(InfrastructureAbstractionsNamespace, true)
                        .Or()
                        .ResideInNamespace(InfrastructureNamespace, true)
                        .Or()
                        .ResideInNamespace(PersistenceEfCoreNamespace, true)
                        .Or()
                        .ResideInNamespace(HttpApiNamespace, true)
                        .Or()
                        .ResideInNamespace(HttpApiPublicNamespace, true)
                )
                .Because("Persistence.DataAccess deve depender apenas de Domain e Domain.Shared");

            efCoreRule.Check(Architecture);
            dataAccessRule.Check(Architecture);
        }

        /// <summary>
        /// Verifica se a camada HttpApi depende apenas de Application, Application.Contracts e Domain.Shared.
        /// </summary>
        [Fact]
        public void HttpApi_Should_Only_Depend_On_Allowed_Layers()
        {
            // HttpApi deve depender apenas de camadas permitidas
            IArchRule rule = Types()
                .That()
                .ResideInNamespace(HttpApiNamespace, true)
                .And()
                .DoNotResideInNamespace(HttpApiPublicNamespace, true) // Excluir HttpApi.Public
                .Should()
                .OnlyDependOn(
                    Types()
                        .That()
                        .ResideInNamespace(HttpApiNamespace, true)
                        .Or()
                        .ResideInNamespace(ApplicationNamespace, true)
                        .Or()
                        .ResideInNamespace(ApplicationContractsNamespace, true)
                        .Or()
                        .ResideInNamespace(DomainSharedNamespace, true)
                        .Or()
                        .ResideInNamespace("System", true)
                        .Or()
                        .ResideInNamespace("Microsoft", true)
                )
                .Because(
                    "HttpApi deve depender apenas de Application, Application.Contracts e Domain.Shared"
                );

            rule.Check(Architecture);
        }

        /// <summary>
        /// Verifica se a camada HttpApi.Public não depende diretamente do Domain.
        /// </summary>
        [Fact]
        public void HttpApi_Public_Should_Not_Depend_Directly_On_Domain()
        {
            // HttpApi.Public não deve depender diretamente do Domain
            IArchRule rule = Types()
                .That()
                .ResideInNamespace(HttpApiPublicNamespace, true)
                .Should()
                .NotDependOnAny(Types().That().ResideInNamespace(DomainNamespace, true))
                .Because("HttpApi.Public não deve depender diretamente do Domain");

            rule.Check(Architecture);
        }
    }
}
