using System;
using System.Collections.Generic;
using System.Linq;
using Testably.Architecture.Testing.Models;
using Testably.Architecture.Testing.TestErrors;

namespace Testably.Architecture.Testing.Internal;

internal class ProjectExpectation : IFilterableProjectExpectation
{
	private readonly List<Project> _projects;
	private readonly TestResultBuilder<ProjectExpectation> _testResultBuilder;

	public ProjectExpectation(IEnumerable<Project> projects)
	{
		_projects = projects.ToList();
		_testResultBuilder = new TestResultBuilder<ProjectExpectation>(this);
	}

	#region IFilterableProjectExpectation Members

	/// <inheritdoc cref="IFilterableProjectExpectation.Which(Func{Project, bool})" />
	public IFilterableProjectExpectation Which(Func<Project, bool> predicate)
	{
		_projects.RemoveAll(p => !predicate(p));
		return this;
	}

	/// <inheritdoc />
	public ITestResult<IProjectExpectation> ShouldSatisfy(
		Func<Project, bool> condition,
		Func<Project, TestError>? errorGenerator = null)
	{
		errorGenerator ??= p =>
			new TestError($"Project '{p.Name}' does not satisfy the required condition");
		foreach (Project project in _projects)
		{
			if (!condition(project))
			{
				TestError error = errorGenerator(project);
				_testResultBuilder.Add(error);
			}
		}

		return _testResultBuilder.Build();
	}

	#endregion
}
