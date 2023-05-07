using System;

namespace Testably.Architecture.Rules;

public static partial class RequirementOnTypeExtensions
{
	/// <summary>
	///     Expect the types to be a class.
	/// </summary>
	/// <param name="this">The <see cref="IRequirement{Type}" />.</param>
	public static IRequirementResult<Type> ShouldBeAClass(
		this IRequirement<Type> @this)
		=> @this.ShouldSatisfy(Requirement.ForType(
			type => type.IsClass,
			type => new TypeTestError(type,
				$"Type '{type.Name}' should be a class.")));

	/// <summary>
	///     Expect the types to not be a class.
	/// </summary>
	/// <param name="this">The <see cref="IRequirement{Type}" />.</param>
	public static IRequirementResult<Type> ShouldNotBeAClass(
		this IRequirement<Type> @this)
		=> @this.ShouldSatisfy(Requirement.ForType(
			type => !type.IsClass,
			type => new TypeTestError(type,
				$"Type '{type.Name}' should not be a class.")));
}
