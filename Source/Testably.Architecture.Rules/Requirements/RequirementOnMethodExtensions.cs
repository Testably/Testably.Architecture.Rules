using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Extension methods for <see cref="IRequirement{MethodInfo}" />.
/// </summary>
public static class RequirementOnMethodExtensions
{
	/// <summary>
	///     The <see cref="MethodInfo" /> should satisfy the given <paramref name="condition" />.
	/// </summary>
	public static IRequirementResult<MethodInfo> ShouldSatisfy(
		this IRequirement<MethodInfo> @this,
		Expression<Func<MethodInfo, bool>> condition)
	{
		Func<MethodInfo, bool> compiledCondition = condition.Compile();
		return @this.ShouldSatisfy(Requirement.ForMethod(
			compiledCondition,
			method => new MethodTestError(method,
				$"The method '{method.Name}' should satisfy the required condition {condition}.")));
	}
}
