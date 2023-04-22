namespace Testably.Architecture.Testing.Internal;

internal class TestResult<TExpectation> : ITestResult<TExpectation>
{
	public TestResult(TExpectation expectation, TestError[] errors)
	{
		And = expectation;
		Errors = errors;
	}

	/// <inheritdoc />
	public TestError[] Errors { get; }

	public bool IsSatisfied => Errors.Length == 0;

	/// <inheritdoc />
	public TExpectation And { get; }
}
