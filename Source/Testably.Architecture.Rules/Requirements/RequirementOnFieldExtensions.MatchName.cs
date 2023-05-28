using System.Reflection;

namespace Testably.Architecture.Rules;

public static partial class RequirementOnFieldExtensions
{
	/// <summary>
	///     Expect the <see cref="MemberInfo.Name" /> of the fields to match the given <paramref name="pattern" />.
	/// </summary>
	/// <param name="this">The <see cref="IRequirement{Field}" />.</param>
	/// <param name="pattern">
	///     The wildcard condition.
	///     <para />
	///     Supports * to match zero or more characters and ? to match exactly one character.
	/// </param>
	/// <param name="ignoreCase">Flag indicating if the comparison should be case sensitive or not.</param>
	public static IRequirementResult<FieldInfo> ShouldMatchName(
		this IRequirement<FieldInfo> @this,
		Match pattern,
		bool ignoreCase = false)
		=> @this.ShouldSatisfy(Requirement.ForField(
			field => pattern.Matches(field.Name, ignoreCase),
			field => new FieldTestError(field,
				$"The field '{field.Name}' should match pattern '{pattern}'.")));

	/// <summary>
	///     Expect the <see cref="MemberInfo.Name" /> of the fields to not match the given <paramref name="pattern" />.
	/// </summary>
	/// <param name="this">The <see cref="IRequirement{Field}" />.</param>
	/// <param name="pattern">
	///     The wildcard condition.
	///     <para />
	///     Supports * to match zero or more characters and ? to match exactly one character.
	/// </param>
	/// <param name="ignoreCase">Flag indicating if the comparison should be case sensitive or not.</param>
	public static IRequirementResult<FieldInfo> ShouldNotMatchName(
		this IRequirement<FieldInfo> @this,
		Match pattern,
		bool ignoreCase = false)
		=> @this.ShouldSatisfy(Requirement.ForField(
			field => !pattern.Matches(field.Name, ignoreCase),
			field => new FieldTestError(field,
				$"The field '{field.Name}' should not match pattern '{pattern}'.")));
}
