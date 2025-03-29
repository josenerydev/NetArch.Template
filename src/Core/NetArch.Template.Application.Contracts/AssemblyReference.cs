using System.Reflection;

namespace NetArch.Template.Application.Contracts;

public static class AssemblyReference
{
    public static Assembly ApplicationContractsAssembly => typeof(AssemblyReference).Assembly;
}
