using System;
using System.Collections.Generic;

namespace Testably.Architecture.Testing.Internal;

internal class TestResult<TType> : IRequirementResult<TType>,
	IExemptionResult<TType>
{
	private readonly List<TestError> _errors;

	public TestResult(IRequirement<TType> expectation, List<TestError> errors)
	{
		_errors = errors;
		And = expectation;
	}

	#region IExemptionResult<TType> Members

	/// <inheritdoc />
	IExemption<TType> IExemptionResult<TType>.And
		=> this;

	#endregion

	#region IRequirementResult<TType> Members

	/// <inheritdoc cref="IExemptionResult{TExpectation}.And" />
	public IRequirement<TType> And { get; }

	/// <inheritdoc cref="ITestResult.Errors" />
	public TestError[] Errors
		=> _errors.ToArray();

	/// <inheritdoc cref="ITestResult.IsSatisfied" />
	public bool IsSatisfied
		=> _errors.Count == 0;

	/// <inheritdoc />
	public IExemptionResult<TType> Unless(Func<TestError, bool> predicate)
	{
		_errors.RemoveAll(e => predicate(e));
		return this;
	}

	#endregion
}
