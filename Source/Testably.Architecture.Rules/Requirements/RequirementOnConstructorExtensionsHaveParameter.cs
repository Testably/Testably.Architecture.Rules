using System.Reflection;

namespace Testably.Architecture.Rules;

public static partial class RequirementOnConstructorExtensions
{
	/// <summary>
	///     The parameters of the <see cref="ConstructorInfo" /> should satisfy the given <paramref name="parameterFilter" />.
	/// </summary>
	public static IRequirementResult<ConstructorInfo> ShouldHave(
		this IRequirement<ConstructorInfo> @this,
		IUnorderedParameterFilterResult parameterFilter)
	{
		return @this.ShouldSatisfy(Requirement.ForConstructor(
			m => parameterFilter.Apply(m.GetParameters()),
			constructor => new ConstructorTestError(constructor,
				$"The constructor '{constructor.Name}' should have a parameter which {parameterFilter}.")));
	}

	/// <summary>
	///     The parameters of the <see cref="ConstructorInfo" /> should satisfy the given <paramref name="parameterFilter" />.
	/// </summary>
	public static IRequirementResult<ConstructorInfo> ShouldHave(
		this IRequirement<ConstructorInfo> @this,
		IOrderedParameterFilterResult parameterFilter)
	{
		return @this.ShouldSatisfy(Requirement.ForConstructor(
			m => parameterFilter.Apply(m.GetParameters()),
			constructor => new ConstructorTestError(constructor,
				$"The constructor '{constructor.Name}' should have parameters whose {parameterFilter.FriendlyName()}.")));
	}

	/// <summary>
	///     The <see cref="ConstructorInfo" /> should have no parameters.
	/// </summary>
	public static IRequirementResult<ConstructorInfo> ShouldHaveNoParameters(
		this IRequirement<ConstructorInfo> @this)
	{
		return @this.ShouldSatisfy(Requirement.ForConstructor(
			constructor => constructor.GetParameters().Length == 0,
			constructor => new ConstructorTestError(constructor,
				$"The constructor '{constructor.Name}' should have no parameters.")));
	}

	/// <summary>
	///     The <see cref="ConstructorInfo" /> should have (at least) <paramref name="minimumCount" /> parameters.
	/// </summary>
	public static IRequirementResult<ConstructorInfo> ShouldHaveParameters(
		this IRequirement<ConstructorInfo> @this,
		int minimumCount = 1)
	{
		return @this.ShouldSatisfy(Requirement.ForConstructor(
			constructor => constructor.GetParameters().Length >= minimumCount,
			constructor => new ConstructorTestError(constructor,
				$"The constructor '{constructor.Name}' should have at least {minimumCount} {(minimumCount > 1 ? "parameters" : "parameter")}.")));
	}
}
