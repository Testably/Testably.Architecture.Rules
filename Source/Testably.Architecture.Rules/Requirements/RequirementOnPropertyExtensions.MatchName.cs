using System.Reflection;

namespace Testably.Architecture.Rules;

public static partial class RequirementOnPropertyExtensions
{
	/// <summary>
	///     Expect the <see cref="MemberInfo.Name" /> of the propertys to match the given <paramref name="pattern" />.
	/// </summary>
	/// <param name="this">The <see cref="IRequirement{Property}" />.</param>
	/// <param name="pattern">
	///     The wildcard condition.
	///     <para />
	///     Supports * to match zero or more characters and ? to match exactly one character.
	/// </param>
	/// <param name="ignoreCase">Flag indicating if the comparison should be case sensitive or not.</param>
	public static IRequirementResult<PropertyInfo> ShouldMatchName(
		this IRequirement<PropertyInfo> @this,
		Match pattern,
		bool ignoreCase = false)
		=> @this.ShouldSatisfy(Requirement.ForProperty(
			property => pattern.Matches(property.Name, ignoreCase),
			property => new PropertyTestError(property,
				$"The property '{property.Name}' should match pattern '{pattern}'.")));

	/// <summary>
	///     Expect the <see cref="MemberInfo.Name" /> of the propertys to not match the given <paramref name="pattern" />.
	/// </summary>
	/// <param name="this">The <see cref="IRequirement{Property}" />.</param>
	/// <param name="pattern">
	///     The wildcard condition.
	///     <para />
	///     Supports * to match zero or more characters and ? to match exactly one character.
	/// </param>
	/// <param name="ignoreCase">Flag indicating if the comparison should be case sensitive or not.</param>
	public static IRequirementResult<PropertyInfo> ShouldNotMatchName(
		this IRequirement<PropertyInfo> @this,
		Match pattern,
		bool ignoreCase = false)
		=> @this.ShouldSatisfy(Requirement.ForProperty(
			property => !pattern.Matches(property.Name, ignoreCase),
			property => new PropertyTestError(property,
				$"The property '{property.Name}' should not match pattern '{pattern}'.")));
}
