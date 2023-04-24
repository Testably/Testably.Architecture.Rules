using System.Globalization;
using System.Linq;
using Testably.Architecture.Testing.Models;
using Testably.Architecture.Testing.TestErrors;

namespace Testably.Architecture.Testing;

/// <summary>
///     Extension methods for <see cref="IProjectExpectation" />.
/// </summary>
public static class ProjectExpectationExtensions
{
	/// <summary>
	///     The project should not have dependencies on any project that starts with the <paramref name="assemblyNamePrefix" />
	///     .
	/// </summary>
	public static ITestResult<IProjectExpectation> ShouldNotHaveDependenciesOn(
		this IProjectExpectation @this, string assemblyNamePrefix, bool ignoreCase = false)
	{
		bool FailCondition(ProjectReference projectReference)
			=> projectReference.Name.StartsWith(assemblyNamePrefix, ignoreCase,
				CultureInfo.InvariantCulture);

		return @this.ShouldSatisfy(
			p => !p.ProjectReferences.Any(FailCondition),
			p => new DependencyTestError(p,
				p.ProjectReferences.Where(FailCondition).ToArray()));
	}
}
