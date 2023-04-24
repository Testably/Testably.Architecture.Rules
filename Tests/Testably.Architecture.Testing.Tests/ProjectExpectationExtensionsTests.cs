using FluentAssertions;
using System.Linq;
using Testably.Architecture.Testing.Internal;
using Testably.Architecture.Testing.TestErrors;
using Testably.Architecture.Testing.Tests.Helpers;
using Xunit;

namespace Testably.Architecture.Testing.Tests;

public class ProjectExpectationExtensionsTests
{
	[Fact]
	public void ShouldNotHaveDependenciesOn_CaseSensitivity_ShouldDefaultToSensitive()
	{
		ProjectMock testProject = new("test.project",
			new ProjectReferenceMock("Incorrect.Dependency"));
		ProjectExpectation sut = new(new[]
		{
			testProject
		});

		ITestResult<IProjectExpectation> result =
			sut.ShouldNotHaveDependenciesOn("iNCORRECT.dEPENDENCY");

		result.IsSatisfied.Should().BeTrue();
	}

	[Fact]
	public void ShouldNotHaveDependenciesOn_ErrorsShouldIncludeNameOfAllFailedReferences()
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

		ITestResult<IProjectExpectation> result =
			sut.ShouldNotHaveDependenciesOn("Incorrect.Dependency");

		result.IsSatisfied.Should().BeFalse();

		result.Errors.Length.Should().Be(1);
		TestError error = result.Errors.Single();
		error.Should().BeOfType<DependencyTestError>();
		DependencyTestError dependencyTestError = (DependencyTestError)error;
		dependencyTestError.Project.Should().Be(testProject);
		dependencyTestError.ProjectReferences.Length.Should().Be(2);
		dependencyTestError.ProjectReferences.Should()
			.Contain(x => x.Name == "Incorrect.Dependency.1");
		dependencyTestError.ProjectReferences.Should()
			.Contain(x => x.Name == "Incorrect.Dependency.2");
	}

	[Fact]
	public void ShouldNotHaveDependenciesOn_WithDependencyStartingWithPrefix_ShouldReturnFalse()
	{
		ProjectMock testProject = new("test.project",
			new ProjectReferenceMock("Incorrect.Dependency.xyz"));
		ProjectExpectation sut = new(new[]
		{
			testProject
		});

		ITestResult<IProjectExpectation> result =
			sut.ShouldNotHaveDependenciesOn("Incorrect.Dependency");

		result.IsSatisfied.Should().BeFalse();
	}

	[Theory]
	[InlineData(true, false)]
	[InlineData(false, true)]
	public void ShouldNotHaveDependenciesOn_WithIgnoreCaseParameter_ShouldConsiderCaseSensitivity(
		bool ignoreCase, bool expectedResult)
	{
		ProjectMock testProject = new("test.project",
			new ProjectReferenceMock("Incorrect.Dependency"));
		ProjectExpectation sut = new(new[]
		{
			testProject
		});

		ITestResult<IProjectExpectation> result =
			sut.ShouldNotHaveDependenciesOn("iNCORRECT.dEPENDENCY", ignoreCase);

		result.IsSatisfied.Should().Be(expectedResult);
	}

	[Fact]
	public void ShouldNotHaveDependenciesOn_WithoutDependencyStartingWithPrefix_ShouldReturnTrue()
	{
		ProjectMock testProject = new("test.project",
			new ProjectReferenceMock("Incorrect.Dependency"));
		ProjectExpectation sut = new(new[]
		{
			testProject
		});

		ITestResult<IProjectExpectation> result =
			sut.ShouldNotHaveDependenciesOn("Incorrect.Dependency.xyz");

		result.IsSatisfied.Should().BeTrue();
	}
}
