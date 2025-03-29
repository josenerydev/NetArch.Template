using System.Reflection;

namespace NetArch.Template.Persistence.EntityFrameworkCore;

public static class AssemblyReference
{
    public static Assembly PersistenceEntityFrameworkCoreAssembly =>
        typeof(AssemblyReference).Assembly;
}
