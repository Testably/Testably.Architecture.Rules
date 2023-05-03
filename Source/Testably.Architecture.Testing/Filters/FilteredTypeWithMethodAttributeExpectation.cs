using System;
using System.Reflection;

namespace Testably.Architecture.Testing.Filters;

/// <summary>
/// TODO VB
/// </summary>
public class FilteredTypeWithMethodAttributeExpectation : FilteredTypeExpectationOrBase
{
	/// <summary>
	/// TODO VB
	/// </summary>
	/// <param name="filterableTypeExpectation"></param>
	/// <param name="predicate"></param>
	public FilteredTypeWithMethodAttributeExpectation(
		IExpectationFilter<Type> filterableTypeExpectation,
		Func<Type, bool> predicate) : base(filterableTypeExpectation, predicate)
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
	public FilteredTypeWithMethodAttributeExpectation OrAttribute<TAttribute>(
		Func<TAttribute, MethodInfo, bool>? predicate = null,
		bool inherit = true) where TAttribute : Attribute
	{
		Predicates.Add(type => type.HasMethodWithAttribute(predicate, inherit));
		return this;
	}
}
