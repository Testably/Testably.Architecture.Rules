using System;
using System.Reflection;

namespace Testably.Architecture.Rules;

public static partial class RequirementOnTypeExtensions
{
	/// <summary>
	///     Expect the <see cref="MemberInfo.Name" /> of the types to match the given <paramref name="pattern" />.
	/// </summary>
	/// <param name="this">The <see cref="IRequirement{Type}" />.</param>
	/// <param name="pattern">
	///     The wildcard condition.
	///     <para />
	///     Supports * to match zero or more characters and ? to match exactly one character.
	/// </param>
	/// <param name="ignoreCase">Flag indicating if the comparison should be case sensitive or not.</param>
	public static IRequirementResult<Type> ShouldMatchName(
		this IRequirement<Type> @this,
		Match pattern,
		bool ignoreCase = false)
		=> @this.ShouldSatisfy(Requirement.ForType(
			type => pattern.Matches(type.Name, ignoreCase),
			type => new TypeTestError(type,
				$"Type '{type.Name}' should match pattern '{pattern}'.")));

	/// <summary>
	///     Expect the <see cref="MemberInfo.Name" /> of the types to not match the given <paramref name="pattern" />.
	/// </summary>
	/// <param name="this">The <see cref="IRequirement{Type}" />.</param>
	/// <param name="pattern">
	///     The wildcard condition.
	///     <para />
	///     Supports * to match zero or more characters and ? to match exactly one character.
	/// </param>
	/// <param name="ignoreCase">Flag indicating if the comparison should be case sensitive or not.</param>
	public static IRequirementResult<Type> ShouldNotMatchName(
		this IRequirement<Type> @this,
		Match pattern,
		bool ignoreCase = false)
		=> @this.ShouldSatisfy(Requirement.ForType(
			type => !pattern.Matches(type.Name, ignoreCase),
			type => new TypeTestError(type,
				$"Type '{type.Name}' not match pattern '{pattern}'.")));
}
