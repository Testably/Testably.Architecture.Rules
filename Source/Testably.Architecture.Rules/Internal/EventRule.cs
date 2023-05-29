using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Testably.Architecture.Rules.Internal;

internal class EventRule : Rule<EventInfo>, IEventExpectation, IEventFilterResult
{
	/// <inheritdoc cref="IRule.Check" />
	public override IRuleCheck Check
		=> new RuleCheck<EventInfo>(Filters, Requirements, Exemptions,
			_ => _.SelectMany(a => a.GetTypes().SelectMany(t => t.GetEvents())));

	public EventRule(params Filter<EventInfo>[] filters)
	{
		Filters.AddRange(filters);
	}

	#region IEventExpectation Members

	/// <inheritdoc cref="IEventFilter.Which(Filter{EventInfo})" />
	public IEventFilterResult Which(Filter<EventInfo> filter)
	{
		Filters.Add(filter);
		return this;
	}

	#endregion

	#region IEventFilterResult Members

	/// <inheritdoc cref="IEventFilterResult.And" />
	public IEventFilter And => this;

	/// <inheritdoc cref="IEventFilterResult.Types" />
	public ITypeExpectation Types
		=> new TypeRule(new EvenTEntityFilter(Filters));

	/// <inheritdoc cref="IFilter{EventInfo}.Applies(EventInfo)" />
	public bool Applies(EventInfo type)
		=> Filters.All(f => f.Applies(type));

	#endregion

	/// <inheritdoc cref="object.ToString()" />
	public override string ToString()
		=> string.Join(" and ", Filters);

	private sealed class EvenTEntityFilter : Filter<Type>
	{
		private readonly List<Filter<EventInfo>> _eventFilters;

		public EvenTEntityFilter(List<Filter<EventInfo>> eventFilters)
		{
			_eventFilters = eventFilters;
		}

		/// <inheritdoc cref="Filter{Type}.Applies(Type)" />
		public override bool Applies(Type type)
		{
			return type.GetEvents().Any(
				@event => _eventFilters.All(
					filter => filter.Applies(@event)));
		}

		/// <inheritdoc cref="object.ToString()" />
		public override string ToString()
			=> $"The type must have an event {string.Join(" and ", _eventFilters)}";
	}
}
