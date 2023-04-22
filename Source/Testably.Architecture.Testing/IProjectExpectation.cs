using System;
using System.Reflection;

namespace Testably.Architecture.Testing;

public interface IProjectExpectation
{
  ITestResult ShouldOnlyHaveDependenciesThatSatisfy(
    Func<AssemblyName, bool> predicate,
    Func<AssemblyName, TestError>? errorGenerator = null);
}
