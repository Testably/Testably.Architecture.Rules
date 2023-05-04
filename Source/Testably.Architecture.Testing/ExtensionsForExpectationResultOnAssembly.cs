using System;
using System.Reflection;
using Testably.Architecture.Testing.TestErrors;

namespace Testably.Architecture.Testing;

/// <summary>
///     Extension methods for <see cref="IExpectationResult{Assembly}" />.
/// </summary>
public static class ExtensionsForExpectationResultOnAssembly
{
	/// <summary>
	///     Defines an exception to rules by allowing dependencies that match the <paramref name="pattern" />.
	/// </summary>
	/// <param name="this">The <see cref="IExpectationResult{Assembly}" />.</param>
	/// <param name="pattern">
	///     The wildcard condition.
	///     <para />
	///     Supports * to match zero or more characters and ? to match exactly one character.
	/// </param>
	/// <param name="ignoreCase">Flag indicating if the comparison should be case sensitive or not.</param>
	public static IExpectationExceptionResult<Assembly> ExceptDependencyOn(
		this IExpectationResult<Assembly> @this,
		Match pattern,
		bool ignoreCase = false)
		=> @this.ExceptDependencyOn((_, assemblyName) =>
			pattern.Matches(assemblyName.Name, ignoreCase));

	/// <summary>
	///     Defines an exception to rules by allowing dependencies that match the <paramref name="predicate" />.
	/// </summary>
	/// <param name="this">The <see cref="IExpectationResult{Assembly}" />.</param>
	/// <param name="predicate"><see cref="DependencyTestError" />s that match the <paramref name="predicate" /> are allowed.</param>
	public static IExpectationExceptionResult<Assembly> ExceptDependencyOn(
		this IExpectationResult<Assembly> @this,
		Func<Assembly, AssemblyName, bool> predicate) =>
		@this.Except(t => t is DependencyTestError dependencyTestError &&
		                  dependencyTestError.Except(predicate));
}
