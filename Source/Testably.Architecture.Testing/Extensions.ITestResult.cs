using System;
using System.Reflection;
using System.Text.RegularExpressions;
using Testably.Architecture.Testing.TestErrors;

namespace Testably.Architecture.Testing;

public static partial class Extensions
{
	/// <summary>
	///     Defines an exception to rules by allowing dependencies that match the <paramref name="wildcardCondition" />.
	/// </summary>
	/// <param name="this">The <see cref="ITestResult{IAssemblyExpectation}" />.</param>
	/// <param name="wildcardCondition">
	///     The wildcard condition.
	///     <para />
	///     Supports * to match zero or more characters and ? to match exactly one character.
	/// </param>
	/// <param name="ignoreCase">Flag indicating if the comparison should be case sensitive or not.</param>
	public static ITestResult<IAssemblyExpectation> ExceptDependencyOn(
		this ITestResult<IAssemblyExpectation> @this, string wildcardCondition,
		bool ignoreCase = false)
	{
		string regex = WildcardToRegular(wildcardCondition, ignoreCase);
		return @this.ExceptDependencyOn((assembly, assemblyName) =>
			assemblyName.Name != null &&
			Regex.IsMatch(assemblyName.Name,
				regex.Replace("{Assembly}", assembly.GetName().Name)));
	}

	/// <summary>
	///     Defines an exception to rules by allowing dependencies that match the <paramref name="predicate" />.
	/// </summary>
	/// <param name="this">The <see cref="ITestResult{IAssemblyExpectation}" />.</param>
	/// <param name="predicate"><see cref="DependencyTestError" />s that match the <paramref name="predicate" /> are allowed.</param>
	public static ITestResult<IAssemblyExpectation> ExceptDependencyOn(
		this ITestResult<IAssemblyExpectation> @this,
		Func<Assembly, AssemblyName, bool> predicate) =>
		@this.Except(t => t is DependencyTestError dependencyTestError &&
		                  dependencyTestError.Except(predicate));
}
