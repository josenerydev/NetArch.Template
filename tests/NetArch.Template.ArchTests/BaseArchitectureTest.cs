using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace NetArch.Template.ArchTests
{
    /// <summary>
    /// Base class for architecture tests with shared resources
    /// </summary>
    public abstract class BaseArchitectureTest
    {
        protected static readonly Architecture Architecture = new ArchLoader()
            .LoadAssemblies(
                typeof(Domain.AssemblyReference).Assembly,
                typeof(Domain.Shared.AssemblyReference).Assembly,
                typeof(Application.AssemblyReference).Assembly,
                typeof(Application.Contracts.AssemblyReference).Assembly,
                typeof(Infrastructure.AssemblyReference).Assembly,
                typeof(Infrastructure.Abstractions.AssemblyReference).Assembly,
                typeof(Persistence.EntityFrameworkCore.AssemblyReference).Assembly,
                typeof(Persistence.DataAccess.AssemblyReference).Assembly,
                typeof(HttpApi.AssemblyReference).Assembly,
                typeof(HttpApi.Public.AssemblyReference).Assembly
            )
            .Build();

        // Definições de namespaces
        protected readonly string DomainSharedNamespace = "NetArch.Template.Domain.Shared";
        protected readonly string DomainNamespace = "NetArch.Template.Domain";
        protected readonly string ApplicationContractsNamespace =
            "NetArch.Template.Application.Contracts";
        protected readonly string ApplicationNamespace = "NetArch.Template.Application";
        protected readonly string InfrastructureAbstractionsNamespace =
            "NetArch.Template.Infrastructure.Abstractions";
        protected readonly string InfrastructureNamespace = "NetArch.Template.Infrastructure";
        protected readonly string PersistenceEfCoreNamespace =
            "NetArch.Template.Persistence.EntityFrameworkCore";
        protected readonly string PersistenceDataAccessNamespace =
            "NetArch.Template.Persistence.DataAccess";
        protected readonly string HttpApiNamespace = "NetArch.Template.HttpApi";
        protected readonly string HttpApiPublicNamespace = "NetArch.Template.HttpApi.Public";

        // Filtros para camadas
        protected IObjectProvider<IType> DomainSharedLayer =>
            Types().That().ResideInNamespace(DomainSharedNamespace, true).As("Domain.Shared Layer");
        protected IObjectProvider<IType> DomainLayer =>
            Types().That().ResideInNamespace(DomainNamespace, true).As("Domain Layer");
        protected IObjectProvider<IType> ApplicationContractsLayer =>
            Types()
                .That()
                .ResideInNamespace(ApplicationContractsNamespace, true)
                .As("Application.Contracts Layer");
        protected IObjectProvider<IType> ApplicationLayer =>
            Types().That().ResideInNamespace(ApplicationNamespace, true).As("Application Layer");
        protected IObjectProvider<IType> InfrastructureAbstractionsLayer =>
            Types()
                .That()
                .ResideInNamespace(InfrastructureAbstractionsNamespace, true)
                .As("Infrastructure.Abstractions Layer");
        protected IObjectProvider<IType> InfrastructureLayer =>
            Types()
                .That()
                .ResideInNamespace(InfrastructureNamespace, true)
                .As("Infrastructure Layer");
        protected IObjectProvider<IType> PersistenceEfCoreLayer =>
            Types()
                .That()
                .ResideInNamespace(PersistenceEfCoreNamespace, true)
                .As("Persistence.EntityFrameworkCore Layer");
        protected IObjectProvider<IType> PersistenceDataAccessLayer =>
            Types()
                .That()
                .ResideInNamespace(PersistenceDataAccessNamespace, true)
                .As("Persistence.DataAccess Layer");
        protected IObjectProvider<IType> HttpApiLayer =>
            Types().That().ResideInNamespace(HttpApiNamespace, true).As("HttpApi Layer");
        protected IObjectProvider<IType> HttpApiPublicLayer =>
            Types()
                .That()
                .ResideInNamespace(HttpApiPublicNamespace, true)
                .As("HttpApi.Public Layer");
    }
}
