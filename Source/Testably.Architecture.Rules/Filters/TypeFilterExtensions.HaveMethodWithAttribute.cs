using System;
using System.Reflection;

namespace Testably.Architecture.Rules;

public static partial class TypeFilterExtensions
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
	public static WithMethodAttributeFilterResult WhichHaveMethodWithAttribute<
		TAttribute>(
		this ITypeFilter @this,
		Func<TAttribute, MethodInfo, bool>? predicate = null,
		bool inherit = true)
		where TAttribute : Attribute
	{
		WithMethodAttributeFilterResult filter = new(
			@this,
			type => type.HasMethodWithAttribute(predicate, inherit));
		return filter;
	}

	/// <summary>
	///     Add additional filters on a <see cref="Type" /> which has a method with an attribute.
	/// </summary>
	public class WithMethodAttributeFilterResult : Filter.OnType
	{
		internal WithMethodAttributeFilterResult(
			ITypeFilter typeFilter,
			Func<Type, bool> predicate) : base(typeFilter, predicate)
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
		public WithMethodAttributeFilterResult OrAttribute<TAttribute>(
			Func<TAttribute, MethodInfo, bool>? predicate = null,
			bool inherit = true) where TAttribute : Attribute
		{
			Predicates.Add(type => type.HasMethodWithAttribute(predicate, inherit));
			return this;
		}
	}
}
