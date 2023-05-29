using Testably.Architecture.Rules.Internal;

namespace Testably.Architecture.Rules;

/// <summary>
///     Starting point for defining rules about the architectural design.
/// </summary>
public static class Expect
{
	/// <summary>
	///     Define rules about the architectural design.
	/// </summary>
	public static IExpectation That
		=> new Expectation();
}
