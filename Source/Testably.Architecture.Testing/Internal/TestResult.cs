namespace Testably.Architecture.Testing.Internal;

internal class TestResult : ITestResult
{
	public TestResult(TestError[] errors)
	{
		Errors = errors;
	}

	#region ITestResult Members

	/// <inheritdoc />
	public TestError[] Errors { get; }

	public bool IsSatisfied => Errors.Length == 0;

	#endregion
}
