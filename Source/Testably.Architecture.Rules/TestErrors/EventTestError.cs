using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     A <see cref="EventInfo" /> for an expectation on an <see cref="TestError" />.
/// </summary>
public class EventTestError : TestError
{
	/// <summary>
	///     The <see cref="EventInfo" /> which does not satisfy all architecture expectations.
	/// </summary>
	public EventInfo Event { get; }

	/// <summary>
	///     Initializes a new instance of <see cref="EventTestError" />.
	/// </summary>
	/// <param name="event">
	///     The <see cref="EventInfo" /> which does not satisfy all architecture expectations.
	/// </param>
	/// <param name="errorMessage">The error message for the <see cref="TestError" />.</param>
	public EventTestError(EventInfo @event, string errorMessage)
		: base(errorMessage)
	{
		Event = @event;
	}
}
