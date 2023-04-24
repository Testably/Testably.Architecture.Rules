using System;
using System.Collections.Generic;
using Testably.Architecture.Testing.Internal;
using Testably.Architecture.Testing.Models;
using Testably.Architecture.Testing.TestErrors;

namespace Testably.Architecture.Testing.Tests.Helpers;

internal class ProjectExpectationMock : IProjectExpectation
{
	private Func<Project, bool>? _condition;
	private Func<Project, TestError>? _errorGenerator;
	private readonly Project[] _projects;

	public ProjectExpectationMock(params Project[] projects)
	{
		_projects = projects;
	}

	#region IProjectExpectation Members

	/// <inheritdoc />
	public ITestResult<IProjectExpectation> ShouldSatisfy(
		Func<Project, bool> condition,
		Func<Project, TestError>? errorGenerator = null)
	{
		_condition = condition;
		_errorGenerator = errorGenerator;
		return new TestResult<ProjectExpectationMock>(this, new List<TestError>());
	}

	#endregion

	public bool TestCondition(Project project)
		=> _condition?.Invoke(project) ?? throw new NotSupportedException(
			"You have to first call 'ShouldSatisfy' in order to test the condition.");

	public TestError TestErrorGenerator(Project project)
	{
		if (_condition == null)
		{
			throw new NotSupportedException(
				"You have to first call 'ShouldSatisfy' in order to test the condition.");
		}

		_errorGenerator ??= p =>
			new TestError($"Mocked project '{p.Name}' does not satisfy the required condition");
		return _errorGenerator.Invoke(project);
	}
}
