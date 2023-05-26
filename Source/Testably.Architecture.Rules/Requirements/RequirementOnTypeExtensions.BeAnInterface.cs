using System;

namespace Testably.Architecture.Rules;

public static partial class RequirementOnTypeExtensions
{
	/// <summary>
	///     Expect the types to be an interface.
	/// </summary>
	/// <param name="this">The <see cref="IRequirement{Type}" />.</param>
	public static IRequirementResult<Type> ShouldBeAnInterface(
		this IRequirement<Type> @this)
		=> @this.ShouldSatisfy(Requirement.ForType(
			type => type.IsInterface,
			type => new TypeTestError(type,
				$"The type '{type.Name}' should be an interface.")));

	/// <summary>
	///     Expect the types to not be an interface.
	/// </summary>
	/// <param name="this">The <see cref="IRequirement{Type}" />.</param>
	public static IRequirementResult<Type> ShouldNotBeAnInterface(
		this IRequirement<Type> @this)
		=> @this.ShouldSatisfy(Requirement.ForType(
			type => !type.IsInterface,
			type => new TypeTestError(type,
				$"The type '{type.Name}' should not be an interface.")));
}
