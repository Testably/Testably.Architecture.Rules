using System.Reflection;

namespace Testably.Architecture.Rules;

public static partial class EventFilterExtensions
{
	/// <summary>
	///     Filter <see cref="EventInfo" />s where the <see cref="MemberInfo.Name" /> matches the given
	///     <paramref name="pattern" />.
	/// </summary>
	public static IEventFilterResult WhichNameMatches(
		this IEventFilter @this,
		Match pattern,
		bool ignoreCase = false)
	{
		return @this.Which(@event => pattern.Matches(@event.Name, ignoreCase));
	}
}
