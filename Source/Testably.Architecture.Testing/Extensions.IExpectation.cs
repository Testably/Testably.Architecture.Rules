using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Testably.Architecture.Testing;

public static partial class Extensions
{
	/// <summary>
	///     Defines expectations on all loaded assemblies from the current <see cref="System.AppDomain.CurrentDomain" />
	/// </summary>
	public static IFilterableAssemblyExpectation AllLoadedAssemblies(
		this IExpectation @this)
		=> @this.Assembly(AppDomain.CurrentDomain.GetAssemblies());

	/// <summary>
	///     Defines expectations on all types from all loaded assemblies from the current <see cref="System.AppDomain.CurrentDomain" />
	/// </summary>
	/// <param name="this"></param>
	/// <returns></returns>
	public static IFilterableTypeExpectation AllLoadedTypes(this IExpectation @this) =>
		@this.Type(AppDomain.CurrentDomain.GetAssemblies()
		   .SelectMany(a => a.GetTypes())
		   .ToArray());

	/// <summary>
	///     Defines expectations on the assembly that contains the <typeparamref name="TAssembly" />.
	/// </summary>
	public static IFilterableAssemblyExpectation AssemblyContaining<TAssembly>(
		this IExpectation @this)
		=> @this.Assembly(typeof(TAssembly).Assembly);

	/// <summary>
	///     Defines expectations on all loaded assemblies that match the <paramref name="wildcardCondition" />.
	/// </summary>
	/// <param name="this">The <see cref="IExpectation" />.</param>
	/// <param name="wildcardCondition">
	///     The wildcard condition.
	///     <para />
	///     Supports * to match zero or more characters and ? to match exactly one character.
	/// </param>
	/// <param name="ignoreCase">Flag indicating if the comparison should be case sensitive or not.</param>
	public static IFilterableAssemblyExpectation AssembliesMatching(
		this IExpectation @this,
		string wildcardCondition,
		bool ignoreCase = false)
	{
		RegexOptions options = ignoreCase
			? RegexOptions.IgnoreCase
			: RegexOptions.None;
		string regex = WildcardToRegular(wildcardCondition);

		return @this.Assembly(AppDomain.CurrentDomain.GetAssemblies()
		   .Where(a => Regex.IsMatch(a.GetName().Name ?? a.ToString(), regex, options))
		   .ToArray());
	}
}
