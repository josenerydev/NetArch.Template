using ArchUnitNET.Fluent;
using ArchUnitNET.xUnit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace NetArch.Template.ArchTests
{
    /// <summary>
    /// Testes que verificam as convenções de nomenclatura seguidas pelo projeto
    /// </summary>
    public class NamingConventionTests : BaseArchitectureTest
    {
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

        /// <summary>
        /// Verifica se controladores da API seguem convenções de nomenclatura adequadas.
        /// Os controladores devem terminar com o sufixo 'Controller' para seguir
        /// as convenções do ASP.NET Core e facilitar a identificação.
        /// </summary>
        [Fact]
        public void Http_Controllers_Should_Follow_Naming_Convention()
        {
            IArchRule controllerNamingRule = Classes()
                .That()
                .ResideInNamespace(HttpApiNamespace, true)
                .And()
                .HaveNameEndingWith("Controller")
                .Should()
                .BeAssignableTo("Microsoft.AspNetCore.Mvc.ControllerBase")
                .Because("Classes terminadas com 'Controller' devem herdar de ControllerBase");

            controllerNamingRule.Check(Architecture);
        }

        /// <summary>
        /// Verifica se todos os DTOs (Data Transfer Objects) seguem convenções de nomenclatura adequadas.
        /// Os DTOs devem ter o sufixo 'Dto' para identificar claramente seu papel.
        /// </summary>
        [Fact]
        public void Data_Transfer_Objects_Should_Follow_Naming_Convention()
        {
            IArchRule dtoNamingRule = Classes()
                .That()
                .ResideInNamespace(ApplicationContractsNamespace, true)
                .And()
                .AreNot(Interfaces())
                .And()
                .HaveNameContaining("Dto")
                .Should()
                .HaveNameEndingWith("Dto")
                .Because("Data Transfer Objects devem terminar com o sufixo 'Dto'");

            dtoNamingRule.Check(Architecture);
        }
    }
}
