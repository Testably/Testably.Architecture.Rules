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
			t => t.GetFields().Any(
				fieldInfo => _predicates.All(
					predicate => predicate.Applies(fieldInfo))),
			ToString());
	}

	#endregion

	/// <inheritdoc cref="object.ToString()" />
	public override string ToString()
		=> string.Join(" and ", _predicates.Select(x => x.ToString()));
}
