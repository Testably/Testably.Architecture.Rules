using System;
using Testably.Architecture.Testing.TestErrors;

namespace Testably.Architecture.Testing;

/// <summary>
///     Allows defining exceptions to expectations, by removing specific errors.
/// </summary>
/// <typeparam name="TType"></typeparam>
public interface IExpectationExemption<TType>
{
	/// <summary>
	///     Allows defining exceptions to expectations, by removing specific errors.
	/// </summary>
	/// <param name="predicate">Errors that match <paramref name="predicate" /> are allowed.</param>
	IExpectationExemptionResult<TType> Unless(Func<TestError, bool> predicate);
}
