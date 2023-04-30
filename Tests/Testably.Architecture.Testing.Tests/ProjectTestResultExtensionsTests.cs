using FluentAssertions;
using System.Linq;
using Testably.Architecture.Testing.Internal;
using Testably.Architecture.Testing.TestErrors;
using Testably.Architecture.Testing.Tests.Helpers;
using Xunit;

namespace Testably.Architecture.Testing.Tests;

public class ProjectTestResultExtensionsTests
{
	[Fact]
	public void ExceptDependencyOn_ShouldFilterOutExactMatch()
	{
		ProjectMock testProject = new("test.project",
			new ProjectReferenceMock("Correct.Dependency"),
			new ProjectReferenceMock("Incorrect.Dependency.1"),
			new ProjectReferenceMock("Incorrect.Dependency.2"),
			new ProjectReferenceMock("Another.Correct.Dependency"));
		ProjectExpectation sut = new(new[]
		{
			testProject
		});

		ITestResult<IProjectExpectation> result = sut
			.ShouldNotHaveDependenciesOn("Incorrect.Dependency*")
			.ExceptDependencyOn("Incorrect.Dependency.1");

		result.IsSatisfied.Should().BeFalse();

		result.Errors.Length.Should().Be(1);
		TestError error = result.Errors.Single();
		error.Should().BeOfType<DependencyTestError>();
		DependencyTestError dependencyTestError = (DependencyTestError)error;
		dependencyTestError.Project.Should().Be(testProject);
		dependencyTestError.ProjectReferences.Length.Should().Be(1);
		dependencyTestError.ProjectReferences.Should()
			.Contain(x => x.Name == "Incorrect.Dependency.2");
	}

	[Fact]
	public void ExceptDependencyOn_WhenFilteringAllErrors_ShouldReturnSuccess()
	{
		ProjectMock testProject = new("test.project",
			new ProjectReferenceMock("Correct.Dependency"),
			new ProjectReferenceMock("Incorrect.Dependency.1"),
			new ProjectReferenceMock("Incorrect.Dependency.2"),
			new ProjectReferenceMock("Another.Correct.Dependency"));
		ProjectExpectation sut = new(new[]
		{
			testProject
		});

		ITestResult<IProjectExpectation> result = sut
			.ShouldNotHaveDependenciesOn("Incorrect.Dependency")
			.ExceptDependencyOn("Incorrect.Dependency.1")
			.ExceptDependencyOn("Incorrect.Dependency.2");

		result.IsSatisfied.Should().BeTrue();
		result.Errors.Length.Should().Be(0);
	}
}
