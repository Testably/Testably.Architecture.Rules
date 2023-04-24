using System;
using Testably.Architecture.Testing.Models;
using Testably.Architecture.Testing.TestErrors;

namespace Testably.Architecture.Testing.Tests.Helpers;

internal class ProjectExpectationMock : IProjectExpectation
{
	private Func<Project, bool>? _condition;
	private Func<Project, TestError>? _errorGenerator;

	#region IProjectExpectation Members

	/// <inheritdoc />
	public ITestResult<IProjectExpectation> ShouldSatisfy(
		Func<Project, bool> condition,
		Func<Project, TestError>? errorGenerator = null)
	{
		_condition = condition;
		_errorGenerator = errorGenerator;
		return new DummyTestResult(this);
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

	private class DummyTestResult : ITestResult<IProjectExpectation>
	{
		public DummyTestResult(IProjectExpectation projectExpectation)
		{
			And = projectExpectation;
		}

		#region ITestResult<IProjectExpectation> Members

		/// <inheritdoc />
		public IProjectExpectation And { get; }

		/// <inheritdoc />
		public TestError[] Errors
			=> throw new NotSupportedException("Dummy test result should not be evaluated!");

		/// <inheritdoc />
		public bool IsSatisfied
			=> throw new NotSupportedException("Dummy test result should not be evaluated!");

		#endregion
	}
}
