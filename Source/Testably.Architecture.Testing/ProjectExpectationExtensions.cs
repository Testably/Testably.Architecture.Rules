using System.Linq;
using System.Text.RegularExpressions;
using Testably.Architecture.Testing.Internal;
using Testably.Architecture.Testing.Models;
using Testably.Architecture.Testing.TestErrors;

namespace Testably.Architecture.Testing;

/// <summary>
///     Extension methods for <see cref="IProjectExpectation" />.
/// </summary>
public static class ProjectExpectationExtensions
{
	/// <summary>
	///     The project should not have dependencies on any project that matches
	///     the <paramref name="wildcardCondition" />.
	/// </summary>
	/// <param name="this">The <see cref="IProjectExpectation" />.</param>
	/// <param name="wildcardCondition">
	///     The wildcard condition.
	///     <para />
	///     Supports * to match zero or more characters and ? to match exactly one character.
	/// </param>
	/// <param name="ignoreCase">Flag indicating if the comparison should be case sensitive or not.</param>
	public static ITestResult<IProjectExpectation> ShouldNotHaveDependenciesOn(
		this IProjectExpectation @this, string wildcardCondition,
		bool ignoreCase = false)
	{
		string regex = WildcardHelpers.WildcardToRegular(wildcardCondition, ignoreCase);

		bool FailCondition(ProjectReference projectReference)
		{
			return Regex.IsMatch(projectReference.Name, regex);
		}

		return @this.ShouldSatisfy(
			p => !p.ProjectReferences.Any(FailCondition),
			p => new DependencyTestError(p,
				p.ProjectReferences.Where(FailCondition).ToArray()));
	}
}
