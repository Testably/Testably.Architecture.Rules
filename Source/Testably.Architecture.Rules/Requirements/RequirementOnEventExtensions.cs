using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Extension events for <see cref="IRequirement{EventInfo}" />.
/// </summary>
public static partial class RequirementOnEventExtensions
{
	/// <summary>
	///     The <see cref="EventInfo" /> should satisfy the given <paramref name="condition" />.
	/// </summary>
	public static IRequirementResult<EventInfo> ShouldSatisfy(
		this IRequirement<EventInfo> @this,
		Expression<Func<EventInfo, bool>> condition)
	{
		Func<EventInfo, bool> compiledCondition = condition.Compile();
		return @this.ShouldSatisfy(Requirement.ForEvent(
			compiledCondition,
			@event => new EventTestError(@event,
				$"The event '{@event.Name}' should satisfy the required condition {condition}.")));
	}
}
