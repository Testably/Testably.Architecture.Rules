using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     A <see cref="TestError" /> for a violated rule on an <see cref="EventInfo" />.
/// </summary>
public class EventTestError : TestError
{
	/// <summary>
	///     The <see cref="EventInfo" /> which does not satisfy all architecture rules.
	/// </summary>
	public EventInfo Event { get; }

	/// <summary>
	///     Initializes a new instance of <see cref="EventTestError" />.
	/// </summary>
	/// <param name="event">
	///     The <see cref="EventInfo" /> which does not satisfy all architecture rules.
	/// </param>
	/// <param name="errorMessage">The error message for the <see cref="TestError" />.</param>
	public EventTestError(EventInfo @event, string errorMessage)
		: base(errorMessage)
	{
		Event = @event;
	}
}
