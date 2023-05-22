using System;
using System.Reflection;

namespace Testably.Architecture.Rules;

public static partial class RequirementOnAssemblyExtensions
{
	/// <summary>
	///     Expect the <see cref="AssemblyName.Name" /> of the assemblies to match the given <paramref name="pattern" />.
	/// </summary>
	/// <param name="this">The <see cref="IRequirement{Assembly}" />.</param>
	/// <param name="pattern">
	///     The wildcard condition.
	///     <para />
	///     Supports * to match zero or more characters and ? to match exactly one character.
	/// </param>
	/// <param name="ignoreCase">Flag indicating if the comparison should be case sensitive or not.</param>
	public static IRequirementResult<Assembly> ShouldMatchName(
		this IRequirement<Assembly> @this,
		Match pattern,
		bool ignoreCase = false)
		=> @this.ShouldSatisfy(Requirement.ForAssembly(
			assembly => pattern.Matches(assembly.GetName().Name, ignoreCase),
			assembly => new AssemblyTestError(assembly,
				$"Assembly '{assembly.GetName().Name}' should match pattern '{pattern}'.")));

	/// <summary>
	///     Expect the <see cref="AssemblyName.Name" /> of the assemblies to not match the given <paramref name="pattern" />.
	/// </summary>
	/// <param name="this">The <see cref="IRequirement{Assembly}" />.</param>
	/// <param name="pattern">
	///     The wildcard condition.
	///     <para />
	///     Supports * to match zero or more characters and ? to match exactly one character.
	/// </param>
	/// <param name="ignoreCase">Flag indicating if the comparison should be case sensitive or not.</param>
	public static IRequirementResult<Assembly> ShouldNotMatchName(
		this IRequirement<Assembly> @this,
		Match pattern,
		bool ignoreCase = false)
		=> @this.ShouldSatisfy(Requirement.ForAssembly(
			assembly => !pattern.Matches(assembly.GetName().Name, ignoreCase),
			assembly => new AssemblyTestError(assembly,
				$"Assembly '{assembly.GetName().Name}' should not match pattern '{pattern}'.")));
}
