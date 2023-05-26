using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Add additional filters on the <see cref="EventInfo" />s.
/// </summary>
public interface IEventFilterResult : IFilter<EventInfo>, IRequirement<EventInfo>
{
	/// <summary>
	///     Add additional filters on the <see cref="EventInfo" />s.
	/// </summary>
	IEventFilter And { get; }

	/// <summary>
	///     Get all types from the filtered events.
	/// </summary>
	ITypeExpectation Types { get; }
}
