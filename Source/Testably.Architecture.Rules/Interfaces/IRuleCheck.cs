using System.Collections.Generic;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Allows checking a <see cref="IRule" /> in given <see cref="Assembly" />s.
/// </summary>
public interface IRuleCheck
{
	/// <summary>
	///     Specifies in which <paramref name="assemblies" /> the rule should be checked.
	/// </summary>
	/// <param name="assemblies">The list of assemblies in which to check the rule.</param>
	/// <param name="excludeSystemAssemblies">
	///     Flag, indicating if system assemblies should be filtered out.
	///     <para />
	///     If set to <see langword="true" /> (default value), no assemblies starting with<br />
	///     - <c>mscorlib</c><br />
	///     - <c>System</c><br />
	///     - <c>xunit</c><br />
	///     are loaded.<br />
	///     Otherwise all assemblies are loaded.
	/// </param>
	ITestResult In(IEnumerable<Assembly> assemblies,
		bool excludeSystemAssemblies = true);
}
