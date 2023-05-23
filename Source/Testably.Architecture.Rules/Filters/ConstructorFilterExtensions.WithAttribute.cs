using System;
using System.Reflection;

namespace Testably.Architecture.Rules;

public static partial class ConstructorFilterExtensions
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
	public static WithAttributeFilterResult WithAttribute<TAttribute>(
		this IConstructorFilter @this,
		Func<TAttribute, bool>? predicate = null)
		where TAttribute : Attribute
	{
		WithAttributeFilterResult filter = new(
			@this);
		filter.OrAttribute(predicate);
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
		public WithAttributeFilterResult OrAttribute<TAttribute>(
			Func<TAttribute, bool>? predicate = null) where TAttribute : Attribute
		{
			Predicates.Add(Filter.FromPredicate<ConstructorInfo>(
				type => type.HasAttribute(predicate),
				$"Constructor should have attribute {typeof(TAttribute).Name}"));
			return this;
		}
	}
}
