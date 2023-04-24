using System.Collections.Generic;
using Testably.Architecture.Testing.TestErrors;

namespace Testably.Architecture.Testing.Internal;

internal class TestResultBuilder<TExpectation>
{
	private readonly List<TestError> _errors = new();
	private readonly TExpectation _expectation;

	public TestResultBuilder(TExpectation expectation)
	{
		_expectation = expectation;
	}

	public TestResultBuilder<TExpectation> Add(TestError error)
	{
		_errors.Add(error);
		return this;
	}

	public ITestResult<TExpectation> Build()
	{
		return new TestResult<TExpectation>(_expectation, _errors.ToArray());
	}
}
