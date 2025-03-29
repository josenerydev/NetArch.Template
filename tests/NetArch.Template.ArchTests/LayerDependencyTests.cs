using ArchUnitNET.Fluent;
using ArchUnitNET.xUnit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace NetArch.Template.ArchTests
{
    /// <summary>
    /// Testes que verificam as regras de dependência entre camadas da arquitetura
    /// </summary>
    public class LayerDependencyTests : BaseArchitectureTest
    {
        /// <summary>
        /// Verifica se a camada de Domain não possui dependências em outras camadas.
        /// O Domain deve depender apenas de Domain.Shared e não deve depender de
        /// nenhuma outra camada para manter a integridade da arquitetura limpa.
        /// </summary>
        [Fact]
        public void Domain_Should_Not_Have_Dependencies_On_Other_Layers()
        {
            IArchRule rule = Types()
                .That()
                .Are(DomainLayer)
                .Should()
                .NotDependOnAny(
                    Types()
                        .That()
                        .Are(ApplicationContractsLayer)
                        .Or()
                        .Are(ApplicationLayer)
                        .Or()
                        .Are(InfrastructureAbstractionsLayer)
                        .Or()
                        .Are(InfrastructureLayer)
                        .Or()
                        .Are(PersistenceEfCoreLayer)
                        .Or()
                        .Are(PersistenceDataAccessLayer)
                        .Or()
                        .Are(HttpApiLayer)
                        .Or()
                        .Are(HttpApiPublicLayer)
                )
                .Because("Domain não deve depender de nenhuma outra camada exceto Domain.Shared");

            rule.Check(Architecture);
        }

        /// <summary>
        /// Verifica se a camada Domain.Shared não depende de nenhuma outra camada.
        /// O Domain.Shared deve ser totalmente independente, contendo apenas tipos
        /// fundamentais como enums, valores de objeto e constantes reutilizadas por toda a solução.
        /// </summary>
        [Fact]
        public void Domain_Shared_Should_Not_Depend_On_Any_Layer()
        {
            IArchRule rule = Types()
                .That()
                .Are(DomainSharedLayer)
                .Should()
                .NotDependOnAny(
                    Types()
                        .That()
                        .Are(DomainLayer)
                        .Or()
                        .Are(ApplicationContractsLayer)
                        .Or()
                        .Are(ApplicationLayer)
                        .Or()
                        .Are(InfrastructureAbstractionsLayer)
                        .Or()
                        .Are(InfrastructureLayer)
                        .Or()
                        .Are(PersistenceEfCoreLayer)
                        .Or()
                        .Are(PersistenceDataAccessLayer)
                        .Or()
                        .Are(HttpApiLayer)
                        .Or()
                        .Are(HttpApiPublicLayer)
                )
                .Because("Domain.Shared deve ser totalmente independente das outras camadas");

            rule.Check(Architecture);
        }

        /// <summary>
        /// Verifica se a camada Infrastructure.Abstractions não depende das outras camadas da aplicação.
        /// Infrastructure.Abstractions pode depender apenas de Domain.Shared.
        /// </summary>
        [Fact]
        public void Infrastructure_Abstractions_Should_Only_Depend_On_Domain_Shared()
        {
            IArchRule rule = Types()
                .That()
                .Are(InfrastructureAbstractionsLayer)
                .Should()
                .NotDependOnAny(
                    Types()
                        .That()
                        .Are(DomainLayer)
                        .Or()
                        .Are(ApplicationContractsLayer)
                        .Or()
                        .Are(ApplicationLayer)
                        .Or()
                        .Are(InfrastructureLayer)
                        .Or()
                        .Are(PersistenceEfCoreLayer)
                        .Or()
                        .Are(PersistenceDataAccessLayer)
                        .Or()
                        .Are(HttpApiLayer)
                        .Or()
                        .Are(HttpApiPublicLayer)
                )
                .Because(
                    "Infrastructure.Abstractions só deve depender de Domain.Shared e bibliotecas externas"
                );

            rule.Check(Architecture);
        }

        /// <summary>
        /// Verifica se a camada Application.Contracts depende apenas de Domain.Shared.
        /// Application.Contracts define as interfaces públicas da aplicação e deve depender
        /// apenas de Domain.Shared para manter baixo acoplamento e facilitar testes.
        /// </summary>
        [Fact]
        public void Application_Contracts_Should_Only_Depend_On_Domain_Shared()
        {
            IArchRule rule = Types()
                .That()
                .Are(ApplicationContractsLayer)
                .Should()
                .NotDependOnAny(
                    Types()
                        .That()
                        .Are(DomainLayer)
                        .Or()
                        .Are(ApplicationLayer)
                        .Or()
                        .Are(InfrastructureAbstractionsLayer)
                        .Or()
                        .Are(InfrastructureLayer)
                        .Or()
                        .Are(PersistenceEfCoreLayer)
                        .Or()
                        .Are(PersistenceDataAccessLayer)
                        .Or()
                        .Are(HttpApiLayer)
                        .Or()
                        .Are(HttpApiPublicLayer)
                )
                .Because(
                    "Application.Contracts só deve depender de Domain.Shared e bibliotecas externas"
                );

            rule.Check(Architecture);
        }

        /// <summary>
        /// Verifica se a camada Application depende apenas das camadas permitidas.
        /// </summary>
        [Fact]
        public void Application_Should_Only_Depend_On_Allowed_Layers()
        {
            IArchRule rule = Types()
                .That()
                .Are(ApplicationLayer)
                .Should()
                .NotDependOnAny(
                    Types()
                        .That()
                        .Are(InfrastructureLayer)
                        .Or()
                        .Are(InfrastructureAbstractionsLayer)
                        .Or()
                        .Are(PersistenceEfCoreLayer)
                        .Or()
                        .Are(PersistenceDataAccessLayer)
                        .Or()
                        .Are(HttpApiLayer)
                        .Or()
                        .Are(HttpApiPublicLayer)
                )
                .Because(
                    "Application só deve depender de Domain, Domain.Shared e Application.Contracts"
                );

            rule.Check(Architecture);
        }

        /// <summary>
        /// Verifica se a camada Infrastructure depende apenas de camadas permitidas.
        /// </summary>
        [Fact]
        public void Infrastructure_Should_Not_Depend_On_Forbidden_Layers()
        {
            IArchRule rule = Types()
                .That()
                .Are(InfrastructureLayer)
                .Should()
                .NotDependOnAny(
                    Types()
                        .That()
                        .Are(ApplicationLayer)
                        .Or()
                        .Are(PersistenceEfCoreLayer)
                        .Or()
                        .Are(PersistenceDataAccessLayer)
                        .Or()
                        .Are(HttpApiLayer)
                        .Or()
                        .Are(HttpApiPublicLayer)
                )
                .Because("Infrastructure não deve depender de Application, Persistence ou HttpApi");

            rule.Check(Architecture);
        }

        /// <summary>
        /// Verifica se as camadas de persistência dependem apenas do domínio.
        /// </summary>
        [Fact]
        public void Persistence_Layers_Should_Only_Depend_On_Domain()
        {
            IArchRule efCoreRule = Types()
                .That()
                .Are(PersistenceEfCoreLayer)
                .Should()
                .NotDependOnAny(
                    Types()
                        .That()
                        .Are(ApplicationLayer)
                        .Or()
                        .Are(ApplicationContractsLayer)
                        .Or()
                        .Are(InfrastructureLayer)
                        .Or()
                        .Are(InfrastructureAbstractionsLayer)
                        .Or()
                        .Are(HttpApiLayer)
                        .Or()
                        .Are(HttpApiPublicLayer)
                        .Or()
                        .Are(PersistenceDataAccessLayer)
                )
                .Because(
                    "Persistence.EntityFrameworkCore deve depender apenas de Domain e Domain.Shared"
                );

            IArchRule dataAccessRule = Types()
                .That()
                .Are(PersistenceDataAccessLayer)
                .Should()
                .NotDependOnAny(
                    Types()
                        .That()
                        .Are(ApplicationLayer)
                        .Or()
                        .Are(ApplicationContractsLayer)
                        .Or()
                        .Are(InfrastructureLayer)
                        .Or()
                        .Are(InfrastructureAbstractionsLayer)
                        .Or()
                        .Are(HttpApiLayer)
                        .Or()
                        .Are(HttpApiPublicLayer)
                        .Or()
                        .Are(PersistenceEfCoreLayer)
                )
                .Because("Persistence.DataAccess deve depender apenas de Domain e Domain.Shared");

            efCoreRule.Check(Architecture);
            dataAccessRule.Check(Architecture);
        }

        /// <summary>
        /// Verifica se a camada HttpApi depende apenas das camadas permitidas.
        /// </summary>
        [Fact]
        public void HttpApi_Should_Not_Depend_On_Forbidden_Layers()
        {
            IArchRule rule = Types()
                .That()
                .Are(HttpApiLayer)
                .Should()
                .NotDependOnAny(
                    Types()
                        .That()
                        .Are(DomainLayer)
                        .Or()
                        .Are(InfrastructureLayer)
                        .Or()
                        .Are(InfrastructureAbstractionsLayer)
                        .Or()
                        .Are(PersistenceEfCoreLayer)
                        .Or()
                        .Are(PersistenceDataAccessLayer)
                        .Or()
                        .Are(HttpApiPublicLayer)
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
            IArchRule rule = Types()
                .That()
                .Are(HttpApiPublicLayer)
                .Should()
                .NotDependOnAny(DomainLayer)
                .Because("HttpApi.Public não deve depender diretamente do Domain");

            rule.Check(Architecture);
        }
    }
}
