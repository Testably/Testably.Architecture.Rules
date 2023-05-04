using System;
using System.Reflection;

namespace Testably.Architecture.Testing;

/// <summary>
///     Extension methods for <see cref="IFilter{Type}" />.
/// </summary>
public static class FilterOnTypeExtensions
{
	/// <summary>
	///     Filters the applicable <see cref="Type" /> on which the expectations should be applied.
	/// </summary>
	/// <param name="this">The <see cref="IFilter{Type}" />.</param>
	/// <param name="filter">The filter to apply on the <see cref="Type" />.</param>
	public static IFilterResult<Type> Which(this IFilter<Type> @this,
		Func<Type, bool> filter)
	{
		return @this.Which(Filter<Type>.FromPredicate(filter));
	}

	/// <summary>
	///     Filter for not public types.
	/// </summary>
	public static IFilterResult<Type> WhichAreNotPublic(
		this IFilter<Type> @this)
	{
		return @this.Which(type => !type.IsPublic);
	}

	/// <summary>
	///     Filter for public types.
	/// </summary>
	public static IFilterResult<Type> WhichArePublic(this IFilter<Type> @this)
	{
		return @this.Which(type => type.IsPublic);
	}

	/// <summary>
	///     Filter <see cref="Type" />s that have an attribute of type <typeparamref name="TAttribute" />.
	/// </summary>
	/// <param name="this">The <see cref="IFilter{Type}" />.</param>
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
	public static FilterOnType.WithAttribute WhichHaveAttribute<TAttribute>(
		this IFilter<Type> @this,
		Func<TAttribute, bool>? predicate = null,
		bool inherit = true)
		where TAttribute : Attribute
	{
		FilterOnType.WithAttribute filter =
			new(@this, type => type.HasAttribute(predicate, inherit));
		return filter;
	}

	/// <summary>
	///     Filter <see cref="Type" />s that have an attribute of type <typeparamref name="TAttribute" />.
	/// </summary>
	/// <param name="this">The <see cref="IFilter{Type}" />.</param>
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
	public static FilterOnType.WithMethodAttribute WhichHaveMethodWithAttribute<
		TAttribute>(
		this IFilter<Type> @this,
		Func<TAttribute, MethodInfo, bool>? predicate = null,
		bool inherit = true)
		where TAttribute : Attribute
	{
		FilterOnType.WithMethodAttribute filter =
			new(@this,
				type => type.HasMethodWithAttribute(predicate, inherit));
		@this.Which(filter);
		return filter;
	}
}
