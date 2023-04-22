using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Testably.Architecture.Testing.Models;

namespace Testably.Architecture.Testing.Internal;

internal class AssembliesProjectExpectation : IProjectExpectation
{
	private readonly IEnumerable<Project> _projects;
	private readonly TestResultBuilder<AssembliesProjectExpectation> _testResultBuilder;

	public AssembliesProjectExpectation(IEnumerable<Assembly> projects)
	{
		_projects = projects.Select(x => new Project(x));
		_testResultBuilder = new TestResultBuilder<AssembliesProjectExpectation>(this);
	}

	#region IProjectExpectation Members

	/// <inheritdoc />
	public ITestResult<IProjectExpectation> ShouldSatisfy(
		Func<Project, bool> condition, Func<Project, TestError>? errorGenerator = null)
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
