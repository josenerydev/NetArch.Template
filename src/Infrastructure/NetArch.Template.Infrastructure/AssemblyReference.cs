using System.Reflection;

namespace NetArch.Template.Infrastructure;

public static class AssemblyReference
{
    public static Assembly InfrastructureAssembly => typeof(AssemblyReference).Assembly;
}
