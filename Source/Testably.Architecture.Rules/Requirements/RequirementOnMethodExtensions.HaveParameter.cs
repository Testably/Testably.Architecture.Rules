using System.Reflection;

namespace Testably.Architecture.Rules;

public static partial class RequirementOnMethodExtensions
{
	/// <summary>
	///     The <see cref="MethodInfo" /> should have no parameters.
	/// </summary>
	public static IRequirementResult<MethodInfo> ShouldHaveNoParameters(
		this IRequirement<MethodInfo> @this)
	{
		return @this.ShouldSatisfy(Requirement.ForMethod(
			method => method.GetParameters().Length == 0,
			method => new MethodTestError(method,
				$"The method '{method.Name}' should have no parameters.")));
	}

	/// <summary>
	///     The <see cref="MethodInfo" /> should have (at least) <paramref name="minimumCount" /> parameters.
	/// </summary>
	public static IRequirementResult<MethodInfo> ShouldHaveParameters(
		this IRequirement<MethodInfo> @this,
		int minimumCount = 1)
	{
		return @this.ShouldSatisfy(Requirement.ForMethod(
			method => method.GetParameters().Length >= minimumCount,
			method => new MethodTestError(method,
				$"The method '{method.Name}' should have at least {minimumCount} parameter{(minimumCount > 1 ? "s" : "")}.")));
	}
}
