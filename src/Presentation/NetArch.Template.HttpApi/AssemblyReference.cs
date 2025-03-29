using System.Reflection;

namespace NetArch.Template.HttpApi
{
    public static class AssemblyReference
    {
        public static Assembly PresentationAssembly => typeof(AssemblyReference).Assembly;
    }
}
