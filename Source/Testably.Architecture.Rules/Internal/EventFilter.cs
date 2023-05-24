using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Testably.Architecture.Rules.Internal;

internal class EventFilter : IEventFilter, IEventFilterResult
{
	private readonly List<Filter<EventInfo>> _predicates = new();

	#region IEventFilter Members

	/// <inheritdoc cref="IEventFilter.Which(Filter{EventInfo})" />
	public IEventFilterResult Which(Filter<EventInfo> filter)
	{
		_predicates.Add(filter);
		return this;
	}

	#endregion

	#region IEventFilterResult Members

	/// <inheritdoc cref="IEventFilterResult.And" />
	public IEventFilter And
		=> this;

	/// <inheritdoc cref="IEventFilterResult.ToTypeFilter()" />
	public Filter<Type> ToTypeFilter()
	{
		return Filter.FromPredicate<Type>(
			t => _predicates.All(p => t.GetEvents().Any(p.Applies)),
			ToString());
	}

	#endregion

	/// <inheritdoc cref="object.ToString()" />
	public override string ToString()
		=> string.Join(" and ", _predicates.Select(x => x.ToString()));
}
