using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Testably.Architecture.Testing;

public static partial class Extensions
{
	/// <summary>
	///     Defines expectations on all loaded projects from the current <see cref="System.AppDomain.CurrentDomain" />
	/// </summary>
	public static IFilterableAssemblyExpectation AllLoadedProjects(
		this IExpectation @this)
		=> @this.Assemblies(AppDomain.CurrentDomain.GetAssemblies());

	public static IFilterableTypeExpectation AllLoadedTypes(this IExpectation @this) =>
		@this.Types(AppDomain.CurrentDomain.GetAssemblies()
		   .SelectMany(a => a.GetTypes())
		   .ToArray());

	/// <summary>
	///     Defines expectations on the project from the assembly that contains the <typeparamref name="TAssembly" />.
	/// </summary>
	public static IFilterableAssemblyExpectation ProjectContaining<TAssembly>(
		this IExpectation @this)
		=> @this.Assemblies(typeof(TAssembly).Assembly);

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
	public static IFilterableAssemblyExpectation ProjectsMatching(
		this IExpectation @this,
		string wildcardCondition,
		bool ignoreCase = false)
	{
		string regex = WildcardToRegular(wildcardCondition, ignoreCase);

		return @this.Assemblies(AppDomain.CurrentDomain.GetAssemblies()
		   .Where(a => Regex.IsMatch(a.GetName().Name ?? a.ToString(), regex))
		   .ToArray());
	}
}
