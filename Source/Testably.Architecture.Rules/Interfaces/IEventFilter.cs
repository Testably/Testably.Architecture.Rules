using System;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Defines expectations on <see cref="EventInfo" /> that can be filtered.
/// </summary>
public interface IEventFilter
{
	/// <summary>
	///     Filters the <see cref="Type.GetEvents()" /> on which the expectations should be applied.
	/// </summary>
	/// <param name="filter">The filter to apply on the <see cref="EventInfo" />.</param>
	IEventFilterResult Which(Filter<EventInfo> filter);
}
