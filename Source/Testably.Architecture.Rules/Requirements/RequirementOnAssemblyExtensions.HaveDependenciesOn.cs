using System.Linq;
using System.Reflection;

namespace Testably.Architecture.Rules;

public static partial class RequirementOnAssemblyExtensions
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
			Requirement.ForAssembly(
				p => !p.GetReferencedAssemblies().Any(FailCondition),
				p => new DependencyTestError(p,
					p.GetReferencedAssemblies().Where(FailCondition).ToArray())));
	}
}
