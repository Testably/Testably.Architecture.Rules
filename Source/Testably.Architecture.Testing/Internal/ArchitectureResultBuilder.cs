using System.Collections.Generic;

namespace Testably.Architecture.Testing.Internal;

internal class ArchitectureResultBuilder<TExpectation>
{
	private readonly TExpectation _expectation;
	private readonly List<TestError> _errors = new();

	public ArchitectureResultBuilder(TExpectation expectation)
	{
		_expectation = expectation;
	}

	public ArchitectureResultBuilder<TExpectation> Add(ITestResult result)
	{
		_errors.AddRange(result.Errors);
		return this;
	}

	public ITestResult<TExpectation> Build()
	{
		return new TestResult<TExpectation>(_expectation, _errors.ToArray());
	}
}
