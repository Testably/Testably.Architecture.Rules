using System;

namespace Testably.Architecture.Rules;

public static partial class RequirementOnTypeExtensions
{
	/// <summary>
	///     Expect the types to be generic.
	/// </summary>
	/// <param name="this">The <see cref="IRequirement{Type}" />.</param>
	public static IRequirementResult<Type> ShouldBeGeneric(
		this IRequirement<Type> @this)
		=> @this.ShouldSatisfy(Requirement.ForType(
			type => type.IsGenericType,
			type => new TypeTestError(type,
				$"Type '{type.Name}' should be generic.")));

	/// <summary>
	///     Expect the types to not be generic.
	/// </summary>
	/// <param name="this">The <see cref="IRequirement{Type}" />.</param>
	public static IRequirementResult<Type> ShouldNotBeGeneric(
		this IRequirement<Type> @this)
		=> @this.ShouldSatisfy(Requirement.ForType(
			type => !type.IsGenericType,
			type => new TypeTestError(type,
				$"Type '{type.Name}' should not be generic.")));
}
