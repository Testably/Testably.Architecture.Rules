using System;

namespace Testably.Architecture.Testing.Filters;

public class FilteredTypeWithAttributeExpectationOr : FilteredTypeExpectationOrBase
{
	public FilteredTypeWithAttributeExpectationOr(
		IFilterableTypeExpectation filterableTypeExpectation,
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
	public FilteredTypeWithAttributeExpectationOr OrAttribute<TAttribute>(
		Func<TAttribute, bool>? predicate = null,
		bool inherit = true) where TAttribute : Attribute
	{
		Predicates.Add(type => type.HasAttribute(predicate, inherit));
		return this;
	}
}
