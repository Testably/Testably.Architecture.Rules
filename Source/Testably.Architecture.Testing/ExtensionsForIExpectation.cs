using System;
using System.Linq;
using System.Reflection;
using Testably.Architecture.Testing.Internal;

namespace Testably.Architecture.Testing;

/// <summary>
///     Extension methods for <see cref="IExpectation" />.
/// </summary>
public static class ExtensionsForIExpectation
{
	/// <summary>
	///     Defines expectations on all loaded assemblies from the current <see cref="System.AppDomain.CurrentDomain" />.
	/// </summary>
	/// <param name="this">The <see cref="IExpectation" />.</param>
	/// <param name="predicate">(optional) A predicate to filter the assemblies.</param>
	/// <param name="excludeSystemAssemblies">
	///     Flag, indicating if system assemblies should be filtered out.
	///     <para />
	///     If set to <see langword="true" /> (default value), no assemblies starting with<br />
	///     - <c>mscorlib</c><br />
	///     - <c>System</c><br />
	///     - <c>xunit</c><br />
	///     are loaded.<br />
	///     Otherwise all assemblies matching the <paramref name="predicate" /> are loaded.
	/// </param>
	public static IFilterableAssemblyExpectation AllLoadedAssemblies(
		this IExpectation @this,
		Func<Assembly, bool>? predicate = null,
		bool excludeSystemAssemblies = true)
	{
		predicate ??= _ => true;
		return @this.Assembly(AppDomain.CurrentDomain.GetAssemblies()
			.Where(assembly =>
				!excludeSystemAssemblies ||
				!ExpectationSettings.IsExcluded(assembly))
			.Where(predicate)
			.ToArray());
	}

	/// <summary>
	///     Defines expectations on all types from
	///     <see cref="AllLoadedAssemblies(IExpectation, Func{Assembly,bool},bool)" />.
	/// </summary>
	public static IFilterableTypeExpectation AllLoadedTypes(this IExpectation @this)
		=> @this.AllLoadedAssemblies().Types;

	/// <summary>
	///     Defines expectations on <see cref="AllLoadedAssemblies(IExpectation, Func{Assembly,bool},bool)" />
	///     that match the <paramref name="pattern" />.
	/// </summary>
	/// <param name="this">The <see cref="IExpectation" />.</param>
	/// <param name="pattern">
	///     The match pattern.
	/// </param>
	/// <param name="ignoreCase">Flag indicating if the comparison should be case sensitive or not.</param>
	public static IFilterableAssemblyExpectation AssembliesMatching(
		this IExpectation @this,
		Match pattern,
		bool ignoreCase = false)
		=> @this.AllLoadedAssemblies(
			assembly => pattern.Matches(assembly.GetName().Name, ignoreCase));

	/// <summary>
	///     Defines expectations on the assembly that contains the <typeparamref name="TAssembly" />.
	/// </summary>
	public static IFilterableAssemblyExpectation AssemblyContaining<TAssembly>(
		this IExpectation @this)
		=> @this.Assembly(typeof(TAssembly).Assembly);
}
