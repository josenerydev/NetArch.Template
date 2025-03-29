using ArchUnitNET.Fluent;
using ArchUnitNET.xUnit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace NetArch.Template.ArchTests
{
    /// <summary>
    /// Testes que verificam a correta segregação de componentes e responsabilidades
    /// </summary>
    public class ComponentSegregationTests : BaseArchitectureTest
    {
        /// <summary>
        /// Verifica se controladores da API são usados apenas na camada de API.
        /// Controladores não devem existir em outras camadas da aplicação.
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
        /// Verifica se serviços de aplicação não fazem acesso direto a dados.
        /// Serviços de aplicação devem usar interfaces de repositório em vez de acessar dados diretamente.
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
    }
}
