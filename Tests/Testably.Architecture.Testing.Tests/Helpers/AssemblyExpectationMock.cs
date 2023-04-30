using System;
using System.Collections.Generic;
using System.Reflection;
using Testably.Architecture.Testing.Internal;
using Testably.Architecture.Testing.TestErrors;

namespace Testably.Architecture.Testing.Tests.Helpers;

internal class AssemblyExpectationMock : IAssemblyExpectation
{
	private Func<Assembly, bool>? _condition;
	private Func<Assembly, TestError>? _errorGenerator;
	private readonly Assembly[] _projects;

	public AssemblyExpectationMock(params Assembly[] projects)
	{
		_projects = projects;
	}

	#region IAssemblyExpectation Members

	/// <inheritdoc />
	public ITestResult<IAssemblyExpectation> ShouldSatisfy(
		Func<Assembly, bool> condition,
		Func<Assembly, TestError>? errorGenerator = null)
	{
		_condition = condition;
		_errorGenerator = errorGenerator;
		return new TestResult<AssemblyExpectationMock>(this, new List<TestError>());
	}

	#endregion

	public bool TestCondition(Assembly project)
		=> _condition?.Invoke(project) ?? throw new NotSupportedException(
			"You have to first call 'ShouldSatisfy' in order to test the condition.");

	public TestError TestErrorGenerator(Assembly project)
	{
		if (_condition == null)
		{
			throw new NotSupportedException(
				"You have to first call 'ShouldSatisfy' in order to test the condition.");
		}

		_errorGenerator ??= p =>
			new TestError(
				$"Mocked project '{p.GetName().Name}' does not satisfy the required condition");
		return _errorGenerator.Invoke(project);
	}
}
