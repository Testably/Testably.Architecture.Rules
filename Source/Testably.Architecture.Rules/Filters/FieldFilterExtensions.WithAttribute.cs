using System;
using System.Reflection;

namespace Testably.Architecture.Rules;

public static partial class FieldFilterExtensions
{
	/// <summary>
	///     Filter <see cref="FieldInfo" />s that have an attribute of type <typeparamref name="TAttribute" />.
	/// </summary>
	/// <param name="this">The <see cref="IFieldFilter" />.</param>
	/// <param name="predicate">
	///     (optional) A predicate to check the attribute values.
	///     <para />
	///     If not set (<see langword="null" />), will only check if the attribute is present.
	/// </param>
	public static WithAttributeFilterResult WithAttribute<TAttribute>(
		this IFieldFilter @this,
		Func<TAttribute, bool>? predicate = null)
		where TAttribute : Attribute
	{
		WithAttributeFilterResult filter = new(
			@this);
		filter.OrAttribute(predicate);
		return filter;
	}

	/// <summary>
	///     Add additional filters on a <see cref="FieldInfo" /> which has an attribute.
	/// </summary>
	public class WithAttributeFilterResult : Filter.OnField
	{
		internal WithAttributeFilterResult(
			IFieldFilter typeFilter) : base(typeFilter)
		{
		}

		/// <summary>
		///     Adds another filter <see cref="FieldInfo" />s for an attribute of type <typeparamref name="TAttribute" />.
		/// </summary>
		/// <param name="predicate">
		///     (optional) A predicate to check the attribute values.
		///     <para />
		///     If not set (<see langword="null" />), will only check if the attribute is present.
		/// </param>
		public WithAttributeFilterResult OrAttribute<TAttribute>(
			Func<TAttribute, bool>? predicate = null) where TAttribute : Attribute
		{
			Predicates.Add(Filter.FromPredicate<FieldInfo>(
				type => type.HasAttribute(predicate),
				$"Field should have attribute {typeof(TAttribute).Name}"));
			return this;
		}
	}
}
