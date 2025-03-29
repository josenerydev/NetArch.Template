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
    }
}
