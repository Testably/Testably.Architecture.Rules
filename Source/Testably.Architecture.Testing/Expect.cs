using Testably.Architecture.Testing.Internal;

namespace Testably.Architecture.Testing;

/// <summary>
///     Starting point for defining architectural expectations.
/// </summary>
public static class Expect
{
	/// <summary>
	///     Definition for expectations on the architectural design.
	/// </summary>
	public static IExpectation That { get; } = new Expectation();
}
