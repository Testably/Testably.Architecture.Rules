using System;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Add additional filters on the <see cref="EventInfo" />s.
/// </summary>
public interface IEventFilterResult
{
	/// <summary>
	///     Add additional filters on the <see cref="EventInfo" />s.
	/// </summary>
	IEventFilter And { get; }

	/// <summary>
	///     Create a <see cref="Filter{Type}" /> which satisfies all event filters.
	/// </summary>
	Filter<Type> ToTypeFilter();
}
