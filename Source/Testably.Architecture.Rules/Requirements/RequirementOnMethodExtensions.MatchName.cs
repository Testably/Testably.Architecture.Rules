using System.Reflection;

namespace Testably.Architecture.Rules;

public static partial class RequirementOnMethodExtensions
{
	/// <summary>
	///     Expect the <see cref="MemberInfo.Name" /> of the methods to match the given <paramref name="pattern" />.
	/// </summary>
	/// <param name="this">The <see cref="IRequirement{Method}" />.</param>
	/// <param name="pattern">
	///     The wildcard condition.
	///     <para />
	///     Supports * to match zero or more characters and ? to match exactly one character.
	/// </param>
	/// <param name="ignoreCase">Flag indicating if the comparison should be case sensitive or not.</param>
	public static IRequirementResult<MethodInfo> ShouldMatchName(
		this IRequirement<MethodInfo> @this,
		Match pattern,
		bool ignoreCase = false)
		=> @this.ShouldSatisfy(Requirement.ForMethod(
			method => pattern.Matches(method.Name, ignoreCase),
			method => new MethodTestError(method,
				$"The method '{method.Name}' should match pattern '{pattern}'.")));

	/// <summary>
	///     Expect the <see cref="MemberInfo.Name" /> of the methods to not match the given <paramref name="pattern" />.
	/// </summary>
	/// <param name="this">The <see cref="IRequirement{Method}" />.</param>
	/// <param name="pattern">
	///     The wildcard condition.
	///     <para />
	///     Supports * to match zero or more characters and ? to match exactly one character.
	/// </param>
	/// <param name="ignoreCase">Flag indicating if the comparison should be case sensitive or not.</param>
	public static IRequirementResult<MethodInfo> ShouldNotMatchName(
		this IRequirement<MethodInfo> @this,
		Match pattern,
		bool ignoreCase = false)
		=> @this.ShouldSatisfy(Requirement.ForMethod(
			method => !pattern.Matches(method.Name, ignoreCase),
			method => new MethodTestError(method,
				$"The method '{method.Name}' should not match pattern '{pattern}'.")));
}
