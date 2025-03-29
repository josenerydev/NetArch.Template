using System.Reflection;

namespace NetArch.Template.Infrastructure.Abstractions;

public static class AssemblyReference
{
    public static Assembly InfrastructureAbstractionsAssembly => typeof(AssemblyReference).Assembly;
}
