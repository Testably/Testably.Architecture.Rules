using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Testably.Architecture.Testing;

/// <summary>
///     Extension methods for <see cref="IRequirement{Assembly}" />.
/// </summary>
public static class RequirementOnAssemblyExtensions
{
	/// <summary>
	///     The assembly should not have dependencies on any assembly that matches
	///     the <paramref name="pattern" />.
	/// </summary>
	/// <param name="this">The <see cref="IAssemblyExpectation" />.</param>
	/// <param name="pattern">
	///     The wildcard condition.
	///     <para />
	///     Supports * to match zero or more characters and ? to match exactly one character.
	/// </param>
	/// <param name="ignoreCase">Flag indicating if the comparison should be case sensitive or not.</param>
	public static IRequirementResult<Assembly> ShouldNotHaveDependenciesOn(
		this IRequirement<Assembly> @this,
		Match pattern,
		bool ignoreCase = false)
	{
		bool FailCondition(AssemblyName referencedAssembly)
		{
			return pattern.Matches(referencedAssembly.Name, ignoreCase);
		}

		return @this.ShouldSatisfy(
			p => !p.GetReferencedAssemblies().Any(FailCondition),
			p => new DependencyTestError(p,
				p.GetReferencedAssemblies().Where(FailCondition).ToArray()));
	}

	/// <summary>
	///     The <see cref="Assembly" /> should satisfy the given <paramref name="condition" />.
	/// </summary>
	public static IRequirementResult<Assembly> ShouldSatisfy(
		this IRequirement<Assembly> @this,
		Expression<Func<Assembly, bool>> condition)
	{
		Func<Assembly, bool> compiledCondition = condition.Compile();
		return @this.ShouldSatisfy(compiledCondition,
			assembly => new TestError(
				$"Assembly '{assembly.GetName().Name}' should satisfy the required condition {condition}."));
	}
}
