using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.xUnit;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace NetArch.Template.ArchTests
{
    /// <summary>
    /// Testes que verificam as regras de dependência das camadas de infraestrutura
    /// </summary>
    public class InfrastructureLayerTests : BaseArchitectureTest
    {
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
            // Infrastructure deve depender apenas de camadas permitidas
            IArchRule rule = Types()
                .That()
                .ResideInNamespace(InfrastructureNamespace, true)
                .Should()
                .OnlyDependOn(
                    Types()
                        .That()
                        .ResideInNamespace(InfrastructureNamespace, true)
                        .Or()
                        .ResideInNamespace(InfrastructureAbstractionsNamespace, true)
                        .Or()
                        .ResideInNamespace(DomainNamespace, true)
                        .Or()
                        .ResideInNamespace(DomainSharedNamespace, true)
                        .Or()
                        .ResideInNamespace(ApplicationContractsNamespace, true)
                        .Or()
                        .ResideInNamespace("System", true)
                        .Or()
                        .ResideInNamespace("Microsoft", true)
                )
                .Because(
                    "Infrastructure deve depender apenas de Infrastructure.Abstractions, Domain, Domain.Shared e Application.Contracts"
                );

            rule.Check(Architecture);
        }
    }
}
