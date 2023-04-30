using System;
using System.Collections.Generic;
using Testably.Architecture.Testing.TestErrors;

namespace Testably.Architecture.Testing.Internal;

internal class TestResult<TExpectation> : ITestResult<TExpectation>
{
	private readonly List<TestError> _errors;

	public TestResult(TExpectation expectation, List<TestError> errors)
	{
		_errors = errors;
		And = expectation;
	}

	#region ITestResult<TExpectation> Members

	/// <inheritdoc cref="ITestResult.Errors" />
	public TestError[] Errors
		=> _errors.ToArray();

	/// <inheritdoc cref="ITestResult.IsSatisfied" />
	public bool IsSatisfied
		=> _errors.Count == 0;

	/// <inheritdoc cref="ITestResult{TExpectation}.And" />
	public TExpectation And { get; }

	/// <inheritdoc cref="ITestResult{TExpectation}.Except(Func{TestError, bool})" />
	public ITestResult<TExpectation> Except(Func<TestError, bool> predicate)
	{
		_errors.RemoveAll(e => predicate(e));
		return this;
	}

	#endregion
}
