using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace NetArch.Template.ArchTests
{
    public class LayerDependencyTests
    {
        private static readonly Architecture Architecture = new ArchLoader()
            .LoadAssemblies(
                typeof(Domain.AssemblyReference).Assembly,
                typeof(Application.AssemblyReference).Assembly,
                typeof(Application.Contracts.AssemblyReference).Assembly,
                typeof(Infrastructure.AssemblyReference).Assembly,
                typeof(Infrastructure.Abstractions.AssemblyReference).Assembly,
                typeof(Persistence.EntityFrameworkCore.AssemblyReference).Assembly,
                typeof(HttpApi.AssemblyReference).Assembly,
                typeof(HttpApi.Public.AssemblyReference).Assembly
            )
            .Build();

        [Fact]
        public void Domain_Should_Not_Have_Dependencies_On_Other_Layers()
        {
            var domainNamespace = "NetArch.Template.Domain";
            var applicationNamespace = "NetArch.Template.Application";
            var applicationContractsNamespace = "NetArch.Template.Application.Contracts";
            var infrastructureNamespace = "NetArch.Template.Infrastructure";
            var infrastructureAbstractionNamespace = "NetArch.Template.Infrastructure.Abstraction";
            var persistenceNamespace = "NetArch.Template.Persistence";
            var httpApiNamespace = "NetArch.Template.HttpApi";
            var hostsNamespace = "NetArch.Template.Hosts";
            var rule = Types()
                .That()
                .ResideInNamespace(domainNamespace, true)
                .Should()
                .NotDependOnAny(
                    Types()
                        .That()
                        .ResideInNamespace(applicationNamespace, true)
                        .Or()
                        .ResideInNamespace(applicationContractsNamespace, true)
                        .Or()
                        .ResideInNamespace(infrastructureNamespace, true)
                        .Or()
                        .ResideInNamespace(infrastructureAbstractionNamespace, true)
                        .Or()
                        .ResideInNamespace(persistenceNamespace, true)
                        .Or()
                        .ResideInNamespace(httpApiNamespace, true)
                        .Or()
                        .ResideInNamespace(hostsNamespace, true)
                );
            rule.Check(Architecture);
        }
    }
}
