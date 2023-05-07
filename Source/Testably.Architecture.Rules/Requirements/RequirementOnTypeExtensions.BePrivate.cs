using System;

namespace Testably.Architecture.Rules;

public static partial class RequirementOnTypeExtensions
{
	/// <summary>
	///     Expect the types to be private.
	/// </summary>
	/// <param name="this">The <see cref="IRequirement{Type}" />.</param>
	public static IRequirementResult<Type> ShouldBePrivate(
		this IRequirement<Type> @this)
		=> @this.ShouldSatisfy(Requirement.ForType(
			type => type.IsNested ? type.IsNestedPrivate : type.IsNotPublic,
			type => new TypeTestError(type,
				$"Type '{type.Name}' should be private.")));

	/// <summary>
	///     Expect the types to not be private.
	/// </summary>
	/// <param name="this">The <see cref="IRequirement{Type}" />.</param>
	public static IRequirementResult<Type> ShouldNotBePrivate(
		this IRequirement<Type> @this)
		=> @this.ShouldSatisfy(Requirement.ForType(
			type => type.IsNested ? !type.IsNestedPrivate : !type.IsNotPublic,
			type => new TypeTestError(type,
				$"Type '{type.Name}' should not be private.")));
}
