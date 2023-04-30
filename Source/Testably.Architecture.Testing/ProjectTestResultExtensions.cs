using System;
using System.Text.RegularExpressions;
using Testably.Architecture.Testing.Internal;
using Testably.Architecture.Testing.Models;
using Testably.Architecture.Testing.TestErrors;

namespace Testably.Architecture.Testing;

/// <summary>
///     Extension methods for <see cref="ITestResult{IProjectExpectation}" />.
/// </summary>
public static class ProjectTestResultExtensions
{
	/// <summary>
	///     Defines an exception to rules by allowing dependencies that match the <paramref name="wildcardCondition" />.
	/// </summary>
	/// <param name="this">The <see cref="ITestResult{IProjectExpectation}" />.</param>
	/// <param name="wildcardCondition">
	///     The wildcard condition.
	///     <para />
	///     Supports * to match zero or more characters and ? to match exactly one character.
	/// </param>
	/// <param name="ignoreCase">Flag indicating if the comparison should be case sensitive or not.</param>
	public static ITestResult<IProjectExpectation> ExceptDependencyOn(
		this ITestResult<IProjectExpectation> @this, string wildcardCondition,
		bool ignoreCase = false)
	{
		string regex = WildcardHelpers.WildcardToRegular(wildcardCondition, ignoreCase);
		return @this.ExceptDependencyOn((p, r) =>
			Regex.IsMatch(r.Name, regex.Replace("{Project}", p.Name)));
	}

	/// <summary>
	///     Defines an exception to rules by allowing dependencies that match the <paramref name="predicate" />.
	/// </summary>
	/// <param name="this">The <see cref="ITestResult{IProjectExpectation}" />.</param>
	/// <param name="predicate"><see cref="DependencyTestError" />s that match the <paramref name="predicate" /> are allowed.</param>
	public static ITestResult<IProjectExpectation> ExceptDependencyOn(
		this ITestResult<IProjectExpectation> @this,
		Func<Project, ProjectReference, bool> predicate) =>
		@this.Except(t => t is DependencyTestError dependencyTestError &&
		                  dependencyTestError.Except(predicate));
}
