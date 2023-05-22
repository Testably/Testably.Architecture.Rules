using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Testably.Architecture.Rules.Internal;

internal class ConstructorFilter : IConstructorFilter, IConstructorFilterResult
{
	private readonly List<Filter<ConstructorInfo>> _predicates = new();

	#region IConstructorFilter Members

	/// <inheritdoc cref="IConstructorFilter.Which(Filter{ConstructorInfo})" />
	public IConstructorFilterResult Which(Filter<ConstructorInfo> filter)
	{
		_predicates.Add(filter);
		return this;
	}

	#endregion

	#region IConstructorFilterResult Members

	/// <inheritdoc cref="IConstructorFilterResult.And" />
	public IConstructorFilter And
		=> this;

	/// <inheritdoc cref="IConstructorFilterResult.ToTypeFilter()" />
	public Filter<Type> ToTypeFilter()
	{
		return Filter.FromPredicate<Type>(
			t => _predicates.All(p => t.GetConstructors().Any(p.Applies)));
	}

	#endregion
}
