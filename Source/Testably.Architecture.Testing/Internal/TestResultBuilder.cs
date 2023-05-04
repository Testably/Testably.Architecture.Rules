using System.Collections.Generic;

namespace Testably.Architecture.Testing.Internal;

internal class TestResultBuilder<TExpectation>
{
	private readonly List<TestError> _errors = new();
	private readonly IRequirement<TExpectation> _expectation;

	public TestResultBuilder(IRequirement<TExpectation> expectation)
	{
		_expectation = expectation;
	}

	public TestResultBuilder<TExpectation> Add(TestError error)
	{
		_errors.Add(error);
		return this;
	}

	public IRequirementResult<TExpectation> Build() =>
		new TestResult<TExpectation>(_expectation, _errors);
}
