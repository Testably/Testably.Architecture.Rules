using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Extension events for <see cref="IEventFilter" />.
/// </summary>
public static partial class EventFilterExtensions
{
	/// <summary>
	///     Filters the applicable <see cref="EventInfo" /> on which the expectations should be applied.
	/// </summary>
	/// <param name="this">The <see cref="IEventFilter" />.</param>
	/// <param name="filter">The filter to apply on the <see cref="EventInfo" />.</param>
	public static IEventFilterResult Which(this IEventFilter @this,
		Expression<Func<EventInfo, bool>> filter)
	{
		return @this.Which(Filter.FromPredicate(filter));
	}

	/// <summary>
	///     Filters the applicable <see cref="EventInfo" /> on which the expectations should be applied.
	/// </summary>
	/// <param name="this">The <see cref="IEventFilter" />.</param>
	/// <param name="filter">The filter to apply on the <see cref="EventInfo" />.</param>
	/// <param name="name">The name of the filter.</param>
	public static IEventFilterResult Which(this IEventFilter @this,
		Func<EventInfo, bool> filter,
		string name)
	{
		return @this.Which(Filter.FromPredicate(filter, name));
	}
}
