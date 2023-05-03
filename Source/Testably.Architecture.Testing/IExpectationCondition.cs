using System;
using Testably.Architecture.Testing.TestErrors;

namespace Testably.Architecture.Testing;

/// <summary>
///     Defines expectations on <see cref="Type" />s.
/// </summary>
public interface IExpectationCondition<T>
{
	/// <summary>
	///     The <see cref="Type" /> should satisfy the given <paramref name="condition" />.
	/// </summary>
	ITestResult<IExpectationCondition<T>> ShouldSatisfy(
		Func<T, bool> condition,
		Func<T, TestError> errorGenerator);
}
