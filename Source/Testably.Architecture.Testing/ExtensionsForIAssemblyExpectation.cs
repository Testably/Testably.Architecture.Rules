using System.Linq;
using System.Reflection;
using Testably.Architecture.Testing.TestErrors;

namespace Testably.Architecture.Testing;

/// <summary>
///     Extension methods for <see cref="IAssemblyExpectation" />.
/// </summary>
public static class ExtensionsForIAssemblyExpectation
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
	public static ITestResult<IAssemblyExpectation> ShouldNotHaveDependenciesOn(
		this IAssemblyExpectation @this,
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
}
