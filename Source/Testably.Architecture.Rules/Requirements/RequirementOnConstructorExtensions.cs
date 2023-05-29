using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Extension constructors for <see cref="IRequirement{ConstructorInfo}" />.
/// </summary>
public static partial class RequirementOnConstructorExtensions
{
	/// <summary>
	///     The <see cref="ConstructorInfo" /> should satisfy the given <paramref name="condition" />.
	/// </summary>
	public static IRequirementResult<ConstructorInfo> ShouldSatisfy(
		this IRequirement<ConstructorInfo> @this,
		Expression<Func<ConstructorInfo, bool>> condition)
	{
		Func<ConstructorInfo, bool> compiledCondition = condition.Compile();
		return @this.ShouldSatisfy(Requirement.ForConstructor(
			compiledCondition,
			constructor => new ConstructorTestError(constructor,
				$"The constructor '{constructor.Name}' should satisfy the required condition {condition}.")));
	}
}
