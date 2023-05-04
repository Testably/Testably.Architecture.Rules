using System;

namespace Testably.Architecture.Testing;

/// <summary>
///     Allows defining exceptions to expectations, by removing specific errors.
/// </summary>
/// <typeparam name="TType"></typeparam>
public interface IExemption<TType>
{
	/// <summary>
	///     Allows defining exceptions to expectations, by removing specific errors.
	/// </summary>
	/// <param name="predicate">Errors that match <paramref name="predicate" /> are allowed.</param>
	IExemptionResult<TType> Unless(Func<TestError, bool> predicate);
}
