using System;
using Testably.Architecture.Testing.TestErrors;

namespace Testably.Architecture.Testing;

/// <summary>
///     Extension methods for <see cref="ITestResult{IProjectExpectation}" />.
/// </summary>
public static class ProjectTestResultExtensions
{
	public static ITestResult<IProjectExpectation> ExceptDependencyOn(
		this ITestResult<IProjectExpectation> @this, string assemblyName, bool ignoreCase = false)
	{
		return @this.Except(t => t is DependencyTestError dependencyTestError &&
		                         dependencyTestError.Except(r => r.Name.Equals(assemblyName,
			                         ignoreCase
				                         ? StringComparison.InvariantCultureIgnoreCase
				                         : StringComparison.InvariantCulture)));
	}
}
