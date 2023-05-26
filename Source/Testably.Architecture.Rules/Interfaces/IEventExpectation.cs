using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Defines expectations on <see cref="EventInfo" />s that can be filtered.
/// </summary>
public interface IEventExpectation : IEventFilter, IRequirement<EventInfo>
{
}
