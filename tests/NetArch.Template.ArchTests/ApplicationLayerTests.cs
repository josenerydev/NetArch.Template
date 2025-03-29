using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.xUnit;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace NetArch.Template.ArchTests
{
    /// <summary>
    /// Testes que verificam as regras de dependência das camadas de aplicação
    /// </summary>
    public class ApplicationLayerTests : BaseArchitectureTest
    {
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
        /// Verifica se serviços de aplicação não fazem acesso direto a dados.
        /// </summary>
        [Fact]
        public void Application_Services_Should_Not_Access_Data_Directly()
        {
            IArchRule applicationServicesNoDataAccessRule = Classes()
                .That()
                .ResideInNamespace(ApplicationNamespace, true)
                .And()
                .HaveNameEndingWith("Service")
                .Should()
                .NotDependOnAny(
                    Types()
                        .That()
                        .ResideInNamespace("System.Data.SqlClient", true)
                        .Or()
                        .ResideInNamespace("Microsoft.EntityFrameworkCore", true)
                        .Or()
                        .ResideInNamespace("Microsoft.Data", true)
                )
                .Because("Serviços de aplicação não devem acessar dados diretamente");

            applicationServicesNoDataAccessRule.Check(Architecture);
        }
    }
}
