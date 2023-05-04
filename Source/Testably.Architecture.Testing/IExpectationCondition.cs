using System;
using Testably.Architecture.Testing.TestErrors;

namespace Testably.Architecture.Testing;

/// <summary>
///     Defines expectations on <typeparamref name="TType" />.
/// </summary>
public interface IExpectationCondition<TType>
{
	/// <summary>
	///     The <typeparamref name="TType" /> should satisfy the given <paramref name="condition" />.
	/// </summary>
	IExpectationResult<TType> ShouldSatisfy(
		Func<TType, bool> condition,
		Func<TType, TestError> errorGenerator);
}
