using System;
using System.Reflection;

namespace Testably.Architecture.Testing;

/// <summary>
///     Defines expectations on project level.
/// </summary>
public interface IProjectExpectation
{
	/// <summary>
	///     The project should only have dependencies on other projects, that satisfy the given <paramref name="condition" />.
	/// </summary>
	ITestResult ShouldOnlyHaveDependenciesThatSatisfy(
		Func<AssemblyName, bool> condition,
		Func<AssemblyName, TestError>? errorGenerator = null);
}
