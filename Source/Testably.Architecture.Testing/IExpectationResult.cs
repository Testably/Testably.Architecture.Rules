using System;
using Testably.Architecture.Testing.TestErrors;

namespace Testably.Architecture.Testing;

/// <summary>
///     The result of a condition on <typeparamref name="TType" />.
/// </summary>
public interface IExpectationResult<TType> : ITestResult
{
	/// <summary>
	///     Add additional conditions for the architecture expectation on <typeparamref name="TType" />.
	/// </summary>
	IExpectationCondition<TType> And { get; }

	/// <summary>
	///     Allows defining exceptions to expectations, by removing specific errors.
	/// </summary>
	/// <param name="predicate">Errors that match <paramref name="predicate" /> are allowed.</param>
	IExpectationExceptionResult<TType> Except(Func<TestError, bool> predicate);
}
