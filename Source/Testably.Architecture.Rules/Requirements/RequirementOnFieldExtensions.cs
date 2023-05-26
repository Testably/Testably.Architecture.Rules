using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Extension fields for <see cref="IRequirement{FieldInfo}" />.
/// </summary>
public static class RequirementOnFieldExtensions
{
	/// <summary>
	///     The <see cref="FieldInfo" /> should satisfy the given <paramref name="condition" />.
	/// </summary>
	public static IRequirementResult<FieldInfo> ShouldSatisfy(
		this IRequirement<FieldInfo> @this,
		Expression<Func<FieldInfo, bool>> condition)
	{
		Func<FieldInfo, bool> compiledCondition = condition.Compile();
		return @this.ShouldSatisfy(Requirement.ForField(
			compiledCondition,
			field => new FieldTestError(field,
				$"The field '{field.Name}' should satisfy the required condition {condition}.")));
	}
}
