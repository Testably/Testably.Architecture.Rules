using System;
using System.Collections.Generic;
using System.Reflection;

namespace Testably.Architecture.Testing.Internal;

internal class CompositeAssemblyContainer : IProjectExpectation
{
	private readonly IEnumerable<IProjectExpectation> _projects;

	public CompositeAssemblyContainer(IEnumerable<IProjectExpectation> projects)
	{
		_projects = projects;
	}

	#region IProjectExpectation Members

	/// <inheritdoc />
	public ITestResult<IProjectExpectation> ShouldSatisfy(
		Func<AssemblyName, bool> condition, Func<AssemblyName, TestError>? errorGenerator = null)
	{
		ArchitectureResultBuilder<CompositeAssemblyContainer>? builder = new(this);
		foreach (IProjectExpectation project in _projects)
		{
			builder.Add(
				project.ShouldSatisfy(condition, errorGenerator));
		}

		return builder.Build();
	}

	#endregion
}
