using System;

namespace Testably.Architecture.Rules;

/// <summary>
///     Extension methods for <see cref="ITypeFilter" />.
/// </summary>
public static partial class FilterOnTypeExtensions
{
	/// <summary>
	///     Filter <see cref="Type" />s that have an attribute of type <typeparamref name="TAttribute" />.
	/// </summary>
	/// <param name="this">The <see cref="ITypeFilter" />.</param>
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
	public static WithAttributeFilterResult WhichHaveAttribute<TAttribute>(
		this ITypeFilter @this,
		Func<TAttribute, bool>? predicate = null,
		bool inherit = true)
		where TAttribute : Attribute
	{
		WithAttributeFilterResult filter =
			new(@this, type => type.HasAttribute(predicate, inherit));
		return filter;
	}

	/// <summary>
	///     Add additional filters on a <see cref="Type" /> which has an attribute.
	/// </summary>
	public class WithAttributeFilterResult : Filter.OnType
	{
		internal WithAttributeFilterResult(
			ITypeFilter expectationFilter,
			Func<Type, bool> predicate) : base(expectationFilter, predicate)
		{
		}

		/// <summary>
		///     Adds another filter <see cref="Type" />s for an attribute of type <typeparamref name="TAttribute" />.
		/// </summary>
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
		public WithAttributeFilterResult OrAttribute<TAttribute>(
			Func<TAttribute, bool>? predicate = null,
			bool inherit = true) where TAttribute : Attribute
		{
			Predicates.Add(type => type.HasAttribute(predicate, inherit));
			return this;
		}
	}
}
