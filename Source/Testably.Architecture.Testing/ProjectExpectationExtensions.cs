using System.Globalization;

namespace Testably.Architecture.Testing;

/// <summary>
///     Extension methods for <see cref="IProjectExpectation" />.
/// </summary>
public static class ProjectExpectationExtensions
{
	/// <summary>
	///     The project should not have dependencies on any project that starts with the <paramref name="assemblyNamePrefix" />.
	/// </summary>
	public static ITestResult<IProjectExpectation> ShouldNotHaveDependenciesOn(
		this IProjectExpectation @this, string assemblyNamePrefix, bool ignoreCase = false)
	{
		return @this.ShouldSatisfy(a =>
			a.Name?.StartsWith(assemblyNamePrefix,
				ignoreCase,
				CultureInfo.InvariantCulture) != true);
	}
}
