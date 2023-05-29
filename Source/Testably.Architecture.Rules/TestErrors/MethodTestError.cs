using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     A <see cref="TestError" /> for a violated rule on a <see cref="MethodInfo" />.
/// </summary>
public class MethodTestError : TestError
{
	/// <summary>
	///     The <see cref="MethodInfo" /> which does not satisfy all architecture rules.
	/// </summary>
	public MethodInfo Method { get; }

	/// <summary>
	///     Initializes a new instance of <see cref="MethodTestError" />.
	/// </summary>
	/// <param name="method">
	///     The <see cref="MethodInfo" /> which does not satisfy all architecture rules.
	/// </param>
	/// <param name="errorMessage">The error message for the <see cref="TestError" />.</param>
	public MethodTestError(MethodInfo method, string errorMessage)
		: base(errorMessage)
	{
		Method = method;
	}
}
