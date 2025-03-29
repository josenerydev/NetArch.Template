using System.Reflection;

namespace NetArch.Template.Application;

public static class AssemblyReference
{
    public static Assembly ApplicationAssembly => typeof(AssemblyReference).Assembly;
}
