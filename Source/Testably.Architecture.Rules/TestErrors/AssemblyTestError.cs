using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     A <see cref="TestError" /> for a violated rule on an <see cref="Assembly" />.
/// </summary>
public class AssemblyTestError : TestError
{
	/// <summary>
	///     The <see cref="System.Reflection.Assembly" /> which does not satisfy all architecture rules.
	/// </summary>
	public Assembly Assembly { get; }

	/// <summary>
	///     Initializes a new instance of <see cref="AssemblyTestError" />.
	/// </summary>
	/// <param name="assembly">
	///     The <see cref="System.Reflection.Assembly" /> which does not satisfy all rules
	///     expectations.
	/// </param>
	/// <param name="errorMessage">The error message for the <see cref="TestError" />.</param>
	public AssemblyTestError(Assembly assembly, string errorMessage)
		: base(errorMessage)
	{
		Assembly = assembly;
	}
}
