using System.Reflection;

namespace NetArch.Template.Persistence.DataAccess;

public static class AssemblyReference
{
    public static Assembly PersistenceDataAccessAssembly => typeof(AssemblyReference).Assembly;
}
