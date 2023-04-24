using FluentAssertions;
using Testably.Architecture.Testing.TestErrors;
using Testably.Architecture.Testing.Tests.Helpers;
using Xunit;

namespace Testably.Architecture.Testing.Tests;

public class ProjectExpectationExtensionsTests
{
	[Fact]
	public void ShouldNotHaveDependenciesOn_CaseSensitivity_ShouldDefaultToSensitive()
	{
		ProjectExpectationMock sut = new();
		ProjectMock testProject = new("test.project",
			new ProjectReferenceMock("Incorrect.Dependency"));

		sut.ShouldNotHaveDependenciesOn("iNCORRECT.dEPENDENCY");

		bool result = sut.TestCondition(testProject);
		result.Should().BeTrue();
	}

	[Fact]
	public void ShouldNotHaveDependenciesOn_ErrorsShouldIncludeNameOfAllFailedReferences()
	{
		ProjectExpectationMock sut = new();
		ProjectMock testProject = new("test.project",
			new ProjectReferenceMock("Correct.Dependency"),
			new ProjectReferenceMock("Incorrect.Dependency.1"),
			new ProjectReferenceMock("Incorrect.Dependency.2"),
			new ProjectReferenceMock("Another.Correct.Dependency"));

		sut.ShouldNotHaveDependenciesOn("Incorrect.Dependency");

		bool result = sut.TestCondition(testProject);
		result.Should().BeFalse();

		TestError error = sut.TestErrorGenerator(testProject);
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
		ProjectExpectationMock sut = new();
		ProjectMock testProject = new("test.project",
			new ProjectReferenceMock("Incorrect.Dependency.xyz"));

		sut.ShouldNotHaveDependenciesOn("Incorrect.Dependency");

		bool result = sut.TestCondition(testProject);
		result.Should().BeFalse();
	}

	[Theory]
	[InlineData(true, false)]
	[InlineData(false, true)]
	public void ShouldNotHaveDependenciesOn_WithIgnoreCaseParameter_ShouldConsiderCaseSensitivity(
		bool ignoreCase, bool expectedResult)
	{
		ProjectExpectationMock sut = new();
		ProjectMock testProject = new("test.project",
			new ProjectReferenceMock("Incorrect.Dependency"));

		sut.ShouldNotHaveDependenciesOn("iNCORRECT.dEPENDENCY", ignoreCase);

		bool result = sut.TestCondition(testProject);
		result.Should().Be(expectedResult);
	}

	[Fact]
	public void ShouldNotHaveDependenciesOn_WithoutDependencyStartingWithPrefix_ShouldReturnTrue()
	{
		ProjectExpectationMock sut = new();
		ProjectMock testProject = new("test.project",
			new ProjectReferenceMock("Incorrect.Dependency"));

		sut.ShouldNotHaveDependenciesOn("Incorrect.Dependency.xyz");

		bool result = sut.TestCondition(testProject);
		result.Should().BeTrue();
	}
}
