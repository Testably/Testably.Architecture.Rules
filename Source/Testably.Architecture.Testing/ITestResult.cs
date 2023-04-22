namespace Testably.Architecture.Testing;

/// <summary>
///     The result of an architecture test.
/// </summary>
public interface ITestResult
{
	/// <summary>
	///     The errors.
	/// </summary>
	TestError[] Errors { get; }

	/// <summary>
	///     Flag indicating, if all expectations were satisfied.
	/// </summary>
	bool IsSatisfied { get; }
}

public interface ITestResult<out TExpectation> : ITestResult
{
	TExpectation And { get; }
}
