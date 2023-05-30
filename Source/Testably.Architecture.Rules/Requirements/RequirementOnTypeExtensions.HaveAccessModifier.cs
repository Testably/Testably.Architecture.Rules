using System;

namespace Testably.Architecture.Rules;

public static partial class RequirementOnTypeExtensions
{
	/// <summary>
	///     Expect the types to have the correct <paramref name="accessModifiers" />.
	/// </summary>
	public static IRequirementResult<Type> ShouldBe(
		this IRequirement<Type> @this,
		AccessModifiers accessModifiers)
		=> @this.ShouldSatisfy(Requirement.ForType(
			type => type.HasAccessModifier(accessModifiers),
			type => new TypeTestError(type,
				$"The type '{type.Name}' should be {accessModifiers}.")));
}
