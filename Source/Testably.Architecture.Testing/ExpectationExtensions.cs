using System;
using System.Linq;
using System.Text.RegularExpressions;
using Testably.Architecture.Testing.Internal;

namespace Testably.Architecture.Testing;

/// <summary>
///     Extension methods for <see cref="IExpectation" />.
/// </summary>
public static class ExpectationExtensions
{
	/// <summary>
	///     Defines expectations on all loaded projects from the current <see cref="System.AppDomain.CurrentDomain" />
	/// </summary>
	public static IFilterableProjectExpectation AllLoadedProjects(this IExpectation @this)
		=> @this.FromAssembly(AppDomain.CurrentDomain.GetAssemblies());

	/// <summary>
	///     Defines expectations on the project from the assembly that contains the <typeparamref name="TAssembly" />.
	/// </summary>
	public static IFilterableProjectExpectation ProjectContaining<TAssembly>(
		this IExpectation @this)
		=> @this.FromAssembly(typeof(TAssembly).Assembly);

	/// <summary>
	///     Defines expectations on all loaded projects that match the <paramref name="wildcardCondition" />.
	/// </summary>
	/// <param name="this">The <see cref="IExpectation" />.</param>
	/// <param name="wildcardCondition">
	///     The wildcard condition.
	///     <para />
	///     Supports * to match zero or more characters and ? to match exactly one character.
	/// </param>
	/// <param name="ignoreCase">Flag indicating if the comparison should be case sensitive or not.</param>
	public static IFilterableProjectExpectation Projects(
		this IExpectation @this,
		string wildcardCondition,
		bool ignoreCase = false)
	{
		string regex = WildcardHelpers.WildcardToRegular(wildcardCondition, ignoreCase);

		return @this.FromAssembly(AppDomain.CurrentDomain.GetAssemblies()
		   .Where(a => Regex.IsMatch(a.GetName().Name ?? a.ToString(), regex))
		   .ToArray());
	}
}
