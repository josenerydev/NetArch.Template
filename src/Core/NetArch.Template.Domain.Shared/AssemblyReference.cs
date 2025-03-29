using System.Reflection;

namespace NetArch.Template.Domain.Shared;

public static class AssemblyReference
{
    public static Assembly DomainSharedAssembly => typeof(AssemblyReference).Assembly;
}
