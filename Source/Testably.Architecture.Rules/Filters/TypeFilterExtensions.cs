using System;
using System.Linq.Expressions;

namespace Testably.Architecture.Rules;

/// <summary>
///     Extension methods for <see cref="ITypeFilter" />.
/// </summary>
public static partial class TypeFilterExtensions
{
	/// <summary>
	///     Filters the applicable <see cref="Type" /> on which the expectations should be applied.
	/// </summary>
	/// <param name="this">The <see cref="ITypeFilter" />.</param>
	/// <param name="filter">The filter to apply on the <see cref="Type" />.</param>
	public static ITypeFilterResult Which(this ITypeFilter @this,
		Expression<Func<Type, bool>> filter)
	{
		return @this.Which(Filter.FromPredicate(filter));
	}

	/// <summary>
	///     Filters the applicable <see cref="Type" /> on which the expectations should be applied.
	/// </summary>
	/// <param name="this">The <see cref="ITypeFilter" />.</param>
	/// <param name="filter">The filter to apply on the <see cref="Type" />.</param>
	/// <param name="name">The name of the filter.</param>
	public static ITypeFilterResult Which(this ITypeFilter @this,
		Func<Type, bool> filter,
		string name)
	{
		return @this.Which(Filter.FromPredicate(filter, name));
	}

	/// <summary>
	///     Filters the applicable <see cref="Type" /> for types which <see cref="Type.GetMethods()" /> satisfy the
	///     <paramref name="methodFilter" />.
	/// </summary>
	/// <param name="this">The <see cref="ITypeFilter" />.</param>
	/// <param name="methodFilter">The filter to apply on the methods of the <see cref="Type" />.</param>
	public static ITypeFilterResult Which(
		this ITypeFilter @this,
		IMethodFilterResult methodFilter)
	{
		return @this.Which(methodFilter.ToTypeFilter());
	}

	/// <summary>
	///     Filters the applicable <see cref="Type" /> for types which <see cref="Type.GetConstructors()" /> satisfy the
	///     <paramref name="constructorFilter" />.
	/// </summary>
	/// <param name="this">The <see cref="ITypeFilter" />.</param>
	/// <param name="constructorFilter">The filter to apply on the constructors of the <see cref="Type" />.</param>
	public static ITypeFilterResult Which(
		this ITypeFilter @this,
		IConstructorFilterResult constructorFilter)
	{
		return @this.Which(constructorFilter.ToTypeFilter());
	}
}
