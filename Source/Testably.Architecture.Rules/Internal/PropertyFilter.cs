using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Testably.Architecture.Rules.Internal;

internal class PropertyFilter : IPropertyFilter, IPropertyFilterResult
{
	private readonly List<Filter<PropertyInfo>> _predicates = new();

	/// <inheritdoc cref="IPropertyFilterResult.And" />
	public IPropertyFilter And
		=> this;

	/// <inheritdoc cref="IPropertyFilterResult.ToTypeFilter()" />
	public Filter<Type> ToTypeFilter()
	{
		return Filter.FromPredicate<Type>(
			t => _predicates.All(p => t.GetProperties().Any(p.Applies)));
	}

	/// <inheritdoc cref="IPropertyFilter.Which(Filter{PropertyInfo})" />
	public IPropertyFilterResult Which(Filter<PropertyInfo> filter)
	{
		_predicates.Add(filter);
		return this;
	}
}
