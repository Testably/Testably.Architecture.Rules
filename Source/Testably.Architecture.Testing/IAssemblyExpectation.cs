using System;
using System.Reflection;
using Testably.Architecture.Testing.TestErrors;

namespace Testably.Architecture.Testing;

/// <summary>
///     Defines expectations on <see cref="Assembly" />s.
/// </summary>
public interface IAssemblyExpectation
{
	/// <summary>
	///     The <see cref="Assembly" /> should satisfy the given <paramref name="condition" />.
	/// </summary>
	ITestResult<IAssemblyExpectation> ShouldSatisfy(
		Func<Assembly, bool> condition,
		Func<Assembly, TestError>? errorGenerator = null);
}
