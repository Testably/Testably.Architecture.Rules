using System;
using System.Linq;
using System.Reflection;

namespace Testably.Architecture.Testing.Internal;

internal class AssemblyContainer : IProjectExpectation
{
    private readonly Assembly _assembly;

    public AssemblyContainer(Assembly assembly)
    {
        _assembly = assembly;
    }

    #region IProjects Members

    /// <inheritdoc />
    public ITestResult ShouldOnlyHaveDependenciesThatSatisfy(
      Func<AssemblyName, bool> predicate,
      Func<AssemblyName, TestError>? errorGenerator = null)
    {
        errorGenerator ??= a =>
          new TestError($"Dependency '{a.Name} does not satisfy the required condition");
        var errors = _assembly.GetReferencedAssemblies()
         .Where(x => !predicate(x))
         .Select(errorGenerator)
         .ToArray();
        return new TestResult(errors);
    }

    #endregion
}