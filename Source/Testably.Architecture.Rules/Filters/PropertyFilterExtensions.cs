using System;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Extension properties for <see cref="IPropertyFilter" />.
/// </summary>
public static class PropertyFilterExtensions
{
	/// <summary>
	///     Filter <see cref="PropertyInfo" />s that have an attribute of type <typeparamref name="TAttribute" />.
	/// </summary>
	/// <param name="this">The <see cref="IPropertyFilter" />.</param>
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
	public static WithAttributeFilterResult WithAttribute<TAttribute>(
		this IPropertyFilter @this,
		Func<TAttribute, bool>? predicate = null,
		bool inherit = true)
		where TAttribute : Attribute
	{
		WithAttributeFilterResult filter = new(
			@this);
		filter.OrAttribute(predicate, inherit);
		return filter;
	}

	/// <summary>
	///     Add additional filters on a <see cref="PropertyInfo" /> which has an attribute.
	/// </summary>
	public class WithAttributeFilterResult : Filter.OnProperty
	{
		internal WithAttributeFilterResult(
			IPropertyFilter typeFilter) : base(typeFilter)
		{
		}

		/// <summary>
		///     Adds another filter <see cref="PropertyInfo" />s for an attribute of type <typeparamref name="TAttribute" />.
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
			Predicates.Add(Filter.FromPredicate<PropertyInfo>(
				type => type.HasAttribute(predicate, inherit),
				$"Property should have attribute {typeof(TAttribute).Name}"));
			return this;
		}
	}
}
