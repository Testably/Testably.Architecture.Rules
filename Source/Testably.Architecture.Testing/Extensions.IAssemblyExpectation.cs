using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Testably.Architecture.Testing.TestErrors;

namespace Testably.Architecture.Testing;

public static partial class Extensions
{
	/// <summary>
	///     The assembly should not have dependencies on any assembly that matches
	///     the <paramref name="wildcardCondition" />.
	/// </summary>
	/// <param name="this">The <see cref="IAssemblyExpectation" />.</param>
	/// <param name="wildcardCondition">
	///     The wildcard condition.
	///     <para />
	///     Supports * to match zero or more characters and ? to match exactly one character.
	/// </param>
	/// <param name="ignoreCase">Flag indicating if the comparison should be case sensitive or not.</param>
	public static ITestResult<IAssemblyExpectation> ShouldNotHaveDependenciesOn(
		this IAssemblyExpectation @this, string wildcardCondition,
		bool ignoreCase = false)
	{
		string regex = WildcardToRegular(wildcardCondition, ignoreCase);

		bool FailCondition(AssemblyName referencedAssembly)
		{
			return referencedAssembly.Name != null &&
			       Regex.IsMatch(referencedAssembly.Name, regex);
		}

		return @this.ShouldSatisfy(
			p => !p.GetReferencedAssemblies().Any(FailCondition),
			p => new DependencyTestError(p,
				p.GetReferencedAssemblies().Where(FailCondition).ToArray()));
	}
}
