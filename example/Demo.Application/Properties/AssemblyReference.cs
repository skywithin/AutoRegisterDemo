using System.Reflection;

namespace Demo.Application;

public class AssemblyReference
{
    // This class is used to reference the assembly 
    // and to make it visible to other assemblies
    // such as tests or other projects

    public static Assembly Assembly => typeof(AssemblyReference).Assembly;
}
