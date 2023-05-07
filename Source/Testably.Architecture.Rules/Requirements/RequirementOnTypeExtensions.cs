using System;
using System.Linq.Expressions;

namespace Testably.Architecture.Rules;

/// <summary>
///     Extension methods for <see cref="IRequirement{Type}" />.
/// </summary>
public static partial class RequirementOnTypeExtensions
{
	/// <summary>
	///     The <see cref="Type" /> should satisfy the given <paramref name="condition" />.
	/// </summary>
	public static IRequirementResult<Type> ShouldSatisfy(
		this IRequirement<Type> @this,
		Expression<Func<Type, bool>> condition)
	{
		Func<Type, bool> compiledCondition = condition.Compile();
		return @this.ShouldSatisfy(Requirement.ForType(
			compiledCondition,
			type => new TypeTestError(type,
				$"Type '{type.FullName}' should satisfy the required condition {condition}.")));
	}
}
