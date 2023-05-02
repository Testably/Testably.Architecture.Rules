using System;
using System.Reflection;
using Testably.Architecture.Testing.Filters;

namespace Testably.Architecture.Testing;

/// <summary>
///     Extension methods for <see cref="IFilterableTypeExpectation" />.
/// </summary>
public static class ExtensionsForIFilterableTypeExpectation
{
	/// <summary>
	///     Filter <see cref="Type" />s that have an attribute of type <typeparamref name="TAttribute" />.
	/// </summary>
	/// <param name="this">The <see cref="IFilterableTypeExpectation" />.</param>
	/// <param name="predicate">
	///     (optional) A predicate to check the attribute values.
	///     <para />
	///     If not set (<see langword="null" />), will only check if the attribute is present.
	/// </param>
	/// <param name="inherit">
	///     <see langword="true" /> to search the inheritance chain to find the attributes; otherwise,
	///     <see langword="false" />.<br />
	///     Defaults to <see langword="true" />
	/// </param>
	public static FilteredTypeWithAttributeExpectationOr WhichHaveAttribute<TAttribute>(
		this IFilterableTypeExpectation @this,
		Func<TAttribute, bool>? predicate = null,
		bool inherit = true)
		where TAttribute : Attribute
	{
		FilteredTypeWithAttributeExpectationOr filter =
			new FilteredTypeWithAttributeExpectationOr(@this,
				type => type.HasAttribute(predicate, inherit));
		@this.Which(filter);
		return filter;
	}

	/// <summary>
	///     Filter <see cref="Type" />s that have an attribute of type <typeparamref name="TAttribute" />.
	/// </summary>
	/// <param name="this">The <see cref="IFilterableTypeExpectation" />.</param>
	/// <param name="predicate">
	///     (optional) A predicate to check the attribute values.
	///     <para />
	///     If not set (<see langword="null" />), will only check if the attribute is present.
	/// </param>
	/// <param name="inherit">
	///     <see langword="true" /> to search the inheritance chain to find the attributes; otherwise,
	///     <see langword="false" />.<br />
	///     Defaults to <see langword="true" />
	/// </param>
	public static FilteredTypeWithMethodAttributeExpectation WhichHaveMethodWithAttribute<
		TAttribute>(
		this IFilterableTypeExpectation @this,
		Func<TAttribute, MethodInfo, bool>? predicate = null,
		bool inherit = true)
		where TAttribute : Attribute
	{
		FilteredTypeWithMethodAttributeExpectation filter =
			new FilteredTypeWithMethodAttributeExpectation(@this,
				type => type.HasMethodWithAttribute(predicate, inherit));
		@this.Which(filter);
		return filter;
	}

	/// <summary>
	///     Filter for public types.
	/// </summary>
	public static IFilteredTypeExpectation WhichArePublic(this IFilterableTypeExpectation @this)
	{
		return @this.Which(type => type.IsPublic);
	}

	/// <summary>
	///     Filters the applicable <see cref="Type" /> on which the expectations should be applied.
	/// </summary>
	/// <param name="this">The <see cref="IFilterableTypeExpectation" />.</param>
	/// <param name="filter">The filter to apply on the <see cref="Type" />.</param>
	public static IFilteredTypeExpectation Which(this IFilterableTypeExpectation @this,
		Func<Type, bool> filter)
	{
		return @this.Which(filter);
	}
}
