using System;
using Testably.Architecture.Testing.TestErrors;

namespace Testably.Architecture.Testing;

/// <summary>
///     Defines expectations on <see cref="Type" />s.
/// </summary>
public interface ITypeExpectation
{
	/// <summary>
	///     The <see cref="Type" /> should satisfy the given <paramref name="condition" />.
	/// </summary>
	ITestResult<ITypeExpectation> ShouldSatisfy(
		Func<Type, bool> condition,
		Func<Type, TestError> errorGenerator);
}
