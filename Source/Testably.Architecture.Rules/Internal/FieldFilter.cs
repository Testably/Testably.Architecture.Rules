using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Testably.Architecture.Rules.Internal;

internal class FieldFilter : IFieldFilter, IFieldFilterResult
{
	private readonly List<Filter<FieldInfo>> _predicates = new();

	#region IFieldFilter Members

	/// <inheritdoc cref="IFieldFilter.Which(Filter{FieldInfo})" />
	public IFieldFilterResult Which(Filter<FieldInfo> filter)
	{
		_predicates.Add(filter);
		return this;
	}

	#endregion

	#region IFieldFilterResult Members

	/// <inheritdoc cref="IFieldFilterResult.And" />
	public IFieldFilter And
		=> this;

	/// <inheritdoc cref="IFieldFilterResult.ToTypeFilter()" />
	public Filter<Type> ToTypeFilter()
	{
		return Filter.FromPredicate<Type>(
			t => _predicates.All(p => t.GetFields().Any(p.Applies)));
	}

	#endregion
}
