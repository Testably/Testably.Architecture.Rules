using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Extension properties for <see cref="IRequirement{PropertyInfo}" />.
/// </summary>
public static class RequirementOnPropertyExtensions
{
	/// <summary>
	///     The <see cref="PropertyInfo" /> should satisfy the given <paramref name="condition" />.
	/// </summary>
	public static IRequirementResult<PropertyInfo> ShouldSatisfy(
		this IRequirement<PropertyInfo> @this,
		Expression<Func<PropertyInfo, bool>> condition)
	{
		Func<PropertyInfo, bool> compiledCondition = condition.Compile();
		return @this.ShouldSatisfy(Requirement.ForProperty(
			compiledCondition,
			property => new PropertyTestError(property,
				$"The property '{property.Name}' should satisfy the required condition {condition}.")));
	}
}
