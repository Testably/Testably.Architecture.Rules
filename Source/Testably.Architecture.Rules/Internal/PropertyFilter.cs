using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Testably.Architecture.Rules.Internal;

internal class PropertyFilter : IPropertyFilter, IPropertyFilterResult
{
	private readonly List<Filter<PropertyInfo>> _predicates = new();

	#region IPropertyFilter Members

	/// <inheritdoc cref="IPropertyFilter.Which(Filter{PropertyInfo})" />
	public IPropertyFilterResult Which(Filter<PropertyInfo> filter)
	{
		_predicates.Add(filter);
		return this;
	}

	#endregion

	#region IPropertyFilterResult Members

	/// <inheritdoc cref="IPropertyFilterResult.And" />
	public IPropertyFilter And
		=> this;

	/// <inheritdoc cref="IPropertyFilterResult.ToTypeFilter()" />
	public Filter<Type> ToTypeFilter()
	{
		return Filter.FromPredicate<Type>(
			t => t.GetProperties().Any(
				propertyInfo => _predicates.All(
					predicate => predicate.Applies(propertyInfo))),
			ToString());
	}

	#endregion

	/// <inheritdoc cref="object.ToString()" />
	public override string ToString()
		=> string.Join(" and ", _predicates.Select(x => x.ToString()));
}
