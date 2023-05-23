using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Extension methods for <see cref="IRequirement{Assembly}" />.
/// </summary>
public static partial class RequirementOnAssemblyExtensions
{
	/// <summary>
	///     The <see cref="Assembly" /> should satisfy the given <paramref name="condition" />.
	/// </summary>
	public static IRequirementResult<Assembly> ShouldSatisfy(
		this IRequirement<Assembly> @this,
		Expression<Func<Assembly, bool>> condition)
	{
		Func<Assembly, bool> compiledCondition = condition.Compile();
		return @this.ShouldSatisfy(
			Requirement.ForAssembly(compiledCondition,
				assembly => new AssemblyTestError(assembly,
					$"Assembly '{assembly.GetName().Name}' should satisfy the required condition {condition}.")));
	}
}
