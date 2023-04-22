using System;
using System.Reflection;

namespace Testably.Architecture.Testing;

/// <summary>
///     Defines expectations on project level.
/// </summary>
public interface IProjectExpectation
{
	/// <summary>
	///     The project should satisfy the given <paramref name="condition" />.
	/// </summary>
	ITestResult<IProjectExpectation> ShouldSatisfy(
		Func<AssemblyName, bool> condition,
		Func<AssemblyName, TestError>? errorGenerator = null);
}
