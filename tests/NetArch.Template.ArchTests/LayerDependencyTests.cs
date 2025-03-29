using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace NetArch.Template.ArchTests
{
    public class LayerDependencyTests
    {
        private static readonly Architecture Architecture = new ArchLoader()
            .LoadAssemblies(
                typeof(Domain.AssemblyReference).Assembly,
                typeof(Application.AssemblyReference).Assembly,
                typeof(Infrastructure.AssemblyReference).Assembly,
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
            var infrastructureNamespace = "NetArch.Template.Infrastructure";
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
                        .ResideInNamespace(infrastructureNamespace, true)
                        .Or()
                        .ResideInNamespace(persistenceNamespace, true)
                        .Or()
                        .ResideInNamespace(httpApiNamespace, true)
                        .Or()
                        .ResideInNamespace(hostsNamespace, true)
                );

            rule.Check(Architecture);
        }

        [Fact]
        public void Domain_Should_Not_Depend_On_Application()
        {
            var rule = Types()
                .That()
                .ResideInNamespace("NetArch.Template.Domain", true)
                .Should()
                .NotDependOnAny(
                    Types().That().ResideInNamespace("NetArch.Template.Application", true)
                );

            rule.Check(Architecture);
        }

        [Fact]
        public void Domain_Should_Not_Depend_On_Infrastructure()
        {
            var rule = Types()
                .That()
                .ResideInNamespace("NetArch.Template.Domain", true)
                .Should()
                .NotDependOnAny(
                    Types().That().ResideInNamespace("NetArch.Template.Infrastructure", true)
                );

            rule.Check(Architecture);
        }
    }
}
