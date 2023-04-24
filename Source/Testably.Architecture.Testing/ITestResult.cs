using System;
using Testably.Architecture.Testing.TestErrors;

namespace Testably.Architecture.Testing;

/// <summary>
///     The result of an architecture test.
/// </summary>
public interface ITestResult
{
	/// <summary>
	///     The errors.
	/// </summary>
	TestError[] Errors { get; }

	/// <summary>
	///     Flag indicating, if all expectations were satisfied.
	/// </summary>
	bool IsSatisfied { get; }
}

/// <summary>
///     The result of an architecture test.
/// </summary>
public interface ITestResult<out TExpectation> : ITestResult
{
	/// <summary>
	///     Allows adding additional expectations to the final <see cref="ITestResult" />.
	/// </summary>
	TExpectation And { get; }

	/// <summary>
	///     Allows defining exceptions to expectations, by removing specific errors.
	/// </summary>
	/// <param name="predicate">Errors that match <paramref name="predicate" /> are allowed.</param>
	ITestResult<TExpectation> Except(Func<TestError, bool> predicate);
}
