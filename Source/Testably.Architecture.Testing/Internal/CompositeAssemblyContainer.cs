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

    #region IProjects Members

    /// <inheritdoc />
    public ITestResult ShouldOnlyHaveDependenciesThatSatisfy(
      Func<AssemblyName, bool> predicate, Func<AssemblyName, TestError>? errorGenerator = null)
    {
        var builder = new ArchitectureResultBuilder();
        foreach (var project in _projects)
        {
            builder.Add(
              project.ShouldOnlyHaveDependenciesThatSatisfy(predicate, errorGenerator));
        }

        return builder.Build();
    }

    #endregion
}