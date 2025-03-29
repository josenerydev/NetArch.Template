using System.Reflection;

namespace NetArch.Template.Domain;

public static class AssemblyReference
{
    public static Assembly DomainAssembly => typeof(AssemblyReference).Assembly;
}
