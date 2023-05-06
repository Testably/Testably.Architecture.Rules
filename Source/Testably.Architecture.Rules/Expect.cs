using Testably.Architecture.Rules.Internal;

namespace Testably.Architecture.Rules;

/// <summary>
///     Starting point for defining architectural expectations.
/// </summary>
public static class Expect
{
	/// <summary>
	///     Definition for expectations on the architectural design.
	/// </summary>
	public static IExpectation That
		=> new Expectation();
}
