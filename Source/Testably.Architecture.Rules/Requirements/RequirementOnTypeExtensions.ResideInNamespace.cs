using System;

namespace Testably.Architecture.Rules;

public static partial class RequirementOnTypeExtensions
{
	/// <summary>
	///     Expect the <see cref="System.Type.Namespace" /> of the types to match the given <paramref name="pattern" />.
	/// </summary>
	/// <param name="this">The <see cref="IRequirement{Type}" />.</param>
	/// <param name="pattern">
	///     The wildcard condition.
	///     <para />
	///     Supports * to match zero or more characters and ? to match exactly one character.
	/// </param>
	/// <param name="ignoreCase">Flag indicating if the comparison should be case sensitive or not.</param>
	public static IRequirementResult<Type> ShouldResideInNamespace(
		this IRequirement<Type> @this,
		Match pattern,
		bool ignoreCase = false)
		=> @this.ShouldSatisfy(Requirement.ForType(
			type => pattern.Matches(type.Namespace, ignoreCase),
			type => new TypeTestError(type,
				$"The namespace of type '{type.Name}' should match pattern '{pattern}'.")));

	/// <summary>
	///     Expect the <see cref="System.Type.Namespace" /> of the types to not match the given <paramref name="pattern" />.
	/// </summary>
	/// <param name="this">The <see cref="IRequirement{Type}" />.</param>
	/// <param name="pattern">
	///     The wildcard condition.
	///     <para />
	///     Supports * to match zero or more characters and ? to match exactly one character.
	/// </param>
	/// <param name="ignoreCase">Flag indicating if the comparison should be case sensitive or not.</param>
	public static IRequirementResult<Type> ShouldNotResideInNamespace(
		this IRequirement<Type> @this,
		Match pattern,
		bool ignoreCase = false)
		=> @this.ShouldSatisfy(Requirement.ForType(
			type => !pattern.Matches(type.Namespace, ignoreCase),
			type => new TypeTestError(type,
				$"The namespace of type '{type.Name}' should not match pattern '{pattern}'.")));
}
