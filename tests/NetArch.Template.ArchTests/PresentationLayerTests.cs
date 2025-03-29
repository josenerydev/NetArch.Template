using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.xUnit;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace NetArch.Template.ArchTests
{
    /// <summary>
    /// Testes que verificam as regras de dependência das camadas de apresentação
    /// </summary>
    public class PresentationLayerTests : BaseArchitectureTest
    {
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

        /// <summary>
        /// Verifica se controladores da API são usados apenas na camada de API.
        /// </summary>
        [Fact]
        public void Controllers_Should_Only_Exist_In_Api_Layer()
        {
            IArchRule controllersOnlyInApiRule = Classes()
                .That()
                .HaveNameEndingWith("Controller")
                .Should()
                .ResideInNamespace(HttpApiNamespace, true)
                .OrShould()
                .ResideInNamespace(HttpApiPublicNamespace, true)
                .Because("Controladores devem existir apenas nas camadas de API");

            controllersOnlyInApiRule.Check(Architecture);
        }

        /// <summary>
        /// Verifica se classes concretas de domínio não são expostas em contratos de API.
        /// As APIs devem usar DTOs em vez de expor diretamente classes de domínio.
        /// </summary>
        [Fact]
        public void Api_Should_Not_Expose_Domain_Classes()
        {
            // Identifica as classes de domínio
            var domainClasses = Classes()
                .That()
                .ResideInNamespace(DomainNamespace, true)
                .And()
                .AreNot(Interfaces());

            // Verifica se os controladores não retornam classes de domínio diretamente
            IArchRule apiContractsNoDomainRule = Classes()
                .That()
                .ResideInNamespace(HttpApiNamespace, true)
                .And()
                .HaveNameEndingWith("Controller")
                .Should()
                .NotDependOnAny(domainClasses)
                .Because("APIs não devem expor classes de domínio diretamente, devem usar DTOs");

            apiContractsNoDomainRule.Check(Architecture);
        }
    }
}
