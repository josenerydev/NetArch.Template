using System.Reflection;

namespace NetArch.Template.HttpApi.Public;

public static class AssemblyReference
{
    public static Assembly HttpApiPublicAssembly => typeof(AssemblyReference).Assembly;
}
