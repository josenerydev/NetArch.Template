using ArchUnitNET.Fluent;
using ArchUnitNET.xUnit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace NetArch.Template.ArchTests;

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

    /// <summary>
    /// Verifica se os serviços de aplicação seguem convenções de nomenclatura adequadas.
    /// Os serviços de aplicação devem terminar com o sufixo 'Service' ou 'AppService'
    /// para identificar claramente seu papel na arquitetura.
    /// </summary>
    [Fact]
    public void Application_Services_Should_Follow_Naming_Convention()
    {
        IArchRule serviceNamingRule = Classes()
            .That()
            .ResideInNamespace(ApplicationNamespace, true)
            .And()
            .HaveNameContaining("Service")
            .Should()
            .HaveNameEndingWith("Service")
            .OrShould()
            .HaveNameEndingWith("AppService")
            .Because("Serviços de aplicação devem terminar com 'Service' ou 'AppService'");

        serviceNamingRule.Check(Architecture);
    }

    /// <summary>
    /// Verifica se classes que implementam lógica de serviço estão no namespace Services.
    /// </summary>
    [Fact]
    public void Service_Classes_Should_Be_In_Services_Namespace()
    {
        // Verifica se classes que terminam com "Service" ou "AppService" estão no namespace Services
        IArchRule serviceLocationRule = Classes()
            .That()
            .ResideInNamespace(ApplicationNamespace, true)
            .And()
            .HaveNameEndingWith("Service")
            .Or()
            .HaveNameEndingWith("AppService")
            .Should()
            .ResideInNamespace(ApplicationNamespace + ".Services", true)
            .Because("Classes com lógica de serviço devem estar no namespace Services");

        serviceLocationRule.Check(Architecture);
    }

    /// <summary>
    /// Verifica se classes no namespace Services seguem a convenção de nomenclatura de serviços.
    /// </summary>
    [Fact]
    public void Services_Namespace_Classes_Should_Follow_Service_Naming_Convention()
    {
        // Verifica se classes no namespace Services seguem a convenção de nomenclatura
        IArchRule serviceNamespaceRule = Classes()
            .That()
            .ResideInNamespace(ApplicationNamespace + ".Services", true)
            .Should()
            .HaveNameEndingWith("Service")
            .OrShould()
            .HaveNameEndingWith("AppService")
            .Because(
                "Classes no namespace Services devem seguir a convenção de nomenclatura de serviços"
            );

        serviceNamespaceRule.Check(Architecture);
    }

    /// <summary>
    /// Verifica se interfaces de serviços de aplicação seguem a convenção de nomenclatura.
    /// Interfaces de serviço devem começar com 'I' e terminar com 'Service' ou 'AppService'.
    /// </summary>
    [Fact]
    public void Application_Service_Interfaces_Should_Follow_Naming_Convention()
    {
        IArchRule serviceInterfaceNamingRule = Interfaces()
            .That()
            .ResideInNamespace(ApplicationContractsNamespace, true)
            .And()
            .HaveNameContaining("Service")
            .Should()
            .HaveNameStartingWith("I")
            .AndShould()
            .HaveNameEndingWith("Service")
            .OrShould()
            .HaveNameEndingWith("AppService")
            .Because(
                "Interfaces de serviço devem começar com 'I' e terminar com 'Service' ou 'AppService'"
            );

        serviceInterfaceNamingRule.Check(Architecture);
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
