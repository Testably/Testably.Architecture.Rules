using Testably.Architecture.Testing.TestErrors;

namespace Testably.Architecture.Testing.Internal;

internal class TestResult<TExpectation> : ITestResult<TExpectation>
{
	public TestResult(TExpectation expectation, TestError[] errors)
	{
		And = expectation;
		Errors = errors;
	}

	#region ITestResult<TExpectation> Members

	/// <inheritdoc cref="ITestResult{TExpectation}.And" />
	public TExpectation And { get; }

	/// <inheritdoc cref="ITestResult.Errors" />
	public TestError[] Errors { get; }

	/// <inheritdoc cref="ITestResult.IsSatisfied" />
	public bool IsSatisfied => Errors.Length == 0;

	#endregion
}
