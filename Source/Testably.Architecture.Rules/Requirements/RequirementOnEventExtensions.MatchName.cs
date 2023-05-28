using System.Reflection;

namespace Testably.Architecture.Rules;

public static partial class RequirementOnEventExtensions
{
	/// <summary>
	///     Expect the <see cref="MemberInfo.Name" /> of the @events to match the given <paramref name="pattern" />.
	/// </summary>
	/// <param name="this">The <see cref="IRequirement{Event}" />.</param>
	/// <param name="pattern">
	///     The wildcard condition.
	///     <para />
	///     Supports * to match zero or more characters and ? to match exactly one character.
	/// </param>
	/// <param name="ignoreCase">Flag indicating if the comparison should be case sensitive or not.</param>
	public static IRequirementResult<EventInfo> ShouldMatchName(
		this IRequirement<EventInfo> @this,
		Match pattern,
		bool ignoreCase = false)
		=> @this.ShouldSatisfy(Requirement.ForEvent(
			@event => pattern.Matches(@event.Name, ignoreCase),
			@event => new EventTestError(@event,
				$"The event '{@event.Name}' should match pattern '{pattern}'.")));

	/// <summary>
	///     Expect the <see cref="MemberInfo.Name" /> of the @events to not match the given <paramref name="pattern" />.
	/// </summary>
	/// <param name="this">The <see cref="IRequirement{Event}" />.</param>
	/// <param name="pattern">
	///     The wildcard condition.
	///     <para />
	///     Supports * to match zero or more characters and ? to match exactly one character.
	/// </param>
	/// <param name="ignoreCase">Flag indicating if the comparison should be case sensitive or not.</param>
	public static IRequirementResult<EventInfo> ShouldNotMatchName(
		this IRequirement<EventInfo> @this,
		Match pattern,
		bool ignoreCase = false)
		=> @this.ShouldSatisfy(Requirement.ForEvent(
			@event => !pattern.Matches(@event.Name, ignoreCase),
			@event => new EventTestError(@event,
				$"The event '{@event.Name}' should not match pattern '{pattern}'.")));
}
