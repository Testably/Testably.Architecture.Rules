using System.Reflection;

namespace Testably.Architecture.Rules;

public static partial class RequirementOnMethodExtensions
{
	/// <summary>
	///     The parameters of the <see cref="MethodInfo" /> should satisfy the given <paramref name="parameterFilter" />.
	/// </summary>
	public static IRequirementResult<MethodInfo> ShouldHave(
		this IRequirement<MethodInfo> @this,
		IUnorderedParameterFilterResult parameterFilter)
	{
		return @this.ShouldSatisfy(Requirement.ForMethod(
			m => parameterFilter.Apply(m.GetParameters()),
			method => new MethodTestError(method,
				$"The method '{method.Name}' should have a parameter which {parameterFilter}.")));
	}

	/// <summary>
	///     The parameters of the <see cref="MethodInfo" /> should satisfy the given <paramref name="parameterFilter" />.
	/// </summary>
	public static IRequirementResult<MethodInfo> ShouldHave(
		this IRequirement<MethodInfo> @this,
		IOrderedParameterFilterResult parameterFilter)
	{
		return @this.ShouldSatisfy(Requirement.ForMethod(
			m => parameterFilter.Apply(m.GetParameters()),
			method => new MethodTestError(method,
				$"The method '{method.Name}' should have parameters whose {parameterFilter.FriendlyName()}.")));
	}

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
				$"The method '{method.Name}' should have at least {minimumCount} {(minimumCount > 1 ? "parameters" : "parameter")}.")));
	}
}
