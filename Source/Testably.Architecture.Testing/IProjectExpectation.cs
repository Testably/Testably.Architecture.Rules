using System;
using Testably.Architecture.Testing.Models;
using Testably.Architecture.Testing.TestErrors;

namespace Testably.Architecture.Testing;

/// <summary>
///     Defines expectations on project level.
/// </summary>
public interface IProjectExpectation
{
	/// <summary>
	///     The project should satisfy the given <paramref name="condition" />.
	/// </summary>
	ITestResult<IProjectExpectation> ShouldSatisfy(
		Func<Project, bool> condition,
		Func<Project, TestError>? errorGenerator = null);
}
