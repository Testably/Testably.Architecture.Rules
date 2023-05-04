using System;
using System.Collections.Generic;
using System.Data;
using Testably.Architecture.Testing.TestErrors;

namespace Testably.Architecture.Testing.Internal;

internal class TestResult<TType> : IExpectationConditionResult<TType>,
	IExpectationExemptionResult<TType>
{
	private readonly List<TestError> _errors;

	public TestResult(IExpectationCondition<TType> expectation, List<TestError> errors)
	{
		_errors = errors;
		And = expectation;
	}


	#region IExpectationExemptionResult<TExpectation> Members
	/// <inheritdoc />
	IExpectationExemption<TType> IExpectationExemptionResult<TType>.And
		=> this;

	#endregion

	#region IExpectationResult<TExpectation> Members

	/// <inheritdoc cref="IExpectationExemptionResult{TExpectation}.And" />
	public IExpectationCondition<TType> And { get; }

	/// <inheritdoc cref="ITestResult.Errors" />
	public TestError[] Errors
		=> _errors.ToArray();

	/// <inheritdoc cref="ITestResult.IsSatisfied" />
	public bool IsSatisfied
		=> _errors.Count == 0;

	/// <inheritdoc />
	public IExpectationExemptionResult<TType> Unless(Func<TestError, bool> predicate)
	{
		_errors.RemoveAll(e => predicate(e));
		return this;
	}

	#endregion
}
