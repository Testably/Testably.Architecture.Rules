using System;
using System.Collections.Generic;
using Testably.Architecture.Testing.TestErrors;

namespace Testably.Architecture.Testing.Internal;

internal class TestResult<TExpectation> : IExpectationResult<TExpectation>,
	IExpectationExceptionResult<TExpectation>
{
	private readonly List<TestError> _errors;

	public TestResult(IExpectationCondition<TExpectation> expectation, List<TestError> errors)
	{
		_errors = errors;
		And = expectation;
	}

	#region IExpectationExceptionResult<TExpectation> Members

	/// <inheritdoc />
	IExpectationResult<TExpectation> IExpectationExceptionResult<TExpectation>.And
		=> this;

	#endregion

	#region IExpectationResult<TExpectation> Members

	/// <inheritdoc cref="IExpectationExceptionResult{TExpectation}.And" />
	public IExpectationCondition<TExpectation> And { get; }

	/// <inheritdoc cref="ITestResult.Errors" />
	public TestError[] Errors
		=> _errors.ToArray();

	/// <inheritdoc cref="ITestResult.IsSatisfied" />
	public bool IsSatisfied
		=> _errors.Count == 0;

	/// <inheritdoc />
	public IExpectationExceptionResult<TExpectation> Except(Func<TestError, bool> predicate)
	{
		_errors.RemoveAll(e => predicate(e));
		return this;
	}

	#endregion
}
