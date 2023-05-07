using System;

namespace Testably.Architecture.Rules;

public static partial class RequirementOnTypeExtensions
{
	/// <summary>
	///     Expect the types to be public.
	/// </summary>
	/// <param name="this">The <see cref="IRequirement{Type}" />.</param>
	public static IRequirementResult<Type> ShouldBePublic(
		this IRequirement<Type> @this)
		=> @this.ShouldSatisfy(Requirement.ForType(
			type => type.IsNested ? type.IsNestedPublic : type.IsPublic,
			type => new TypeTestError(type,
				$"Type '{type.Name}' should be public.")));

	/// <summary>
	///     Expect the types to not be public.
	/// </summary>
	/// <param name="this">The <see cref="IRequirement{Type}" />.</param>
	public static IRequirementResult<Type> ShouldNotBePublic(
		this IRequirement<Type> @this)
		=> @this.ShouldSatisfy(Requirement.ForType(
			type => type.IsNested ? !type.IsNestedPublic : !type.IsPublic,
			type => new TypeTestError(type,
				$"Type '{type.Name}' should not be public.")));
}
