using System;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Extension constructors for <see cref="IConstructorFilter" />.
/// </summary>
public static class ConstructorFilterExtensions
{
	/// <summary>
	///     Filter <see cref="ConstructorInfo" />s that have an attribute of type <typeparamref name="TAttribute" />.
	/// </summary>
	/// <param name="this">The <see cref="IConstructorFilter" />.</param>
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
		this IConstructorFilter @this,
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
	///     Add additional filters on a <see cref="ConstructorInfo" /> which has an attribute.
	/// </summary>
	public class WithAttributeFilterResult : Filter.OnConstructor
	{
		internal WithAttributeFilterResult(
			IConstructorFilter typeFilter) : base(typeFilter)
		{
		}

		/// <summary>
		///     Adds another filter <see cref="ConstructorInfo" />s for an attribute of type <typeparamref name="TAttribute" />.
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
			Predicates.Add(Filter.FromPredicate<ConstructorInfo>(
				type => type.HasAttribute(predicate, inherit),
				$"Constructor should have attribute {typeof(TAttribute).Name}"));
			return this;
		}
	}
}
