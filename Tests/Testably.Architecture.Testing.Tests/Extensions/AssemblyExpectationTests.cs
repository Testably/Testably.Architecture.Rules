using FluentAssertions;
using System.Linq;
using System.Reflection;
using Testably.Abstractions.Testing;
using Testably.Architecture.Testing.TestErrors;
using Xunit;

namespace Testably.Architecture.Testing.Tests.Extensions;

public class AssemblyExpectationTests
{
	[Fact]
	public void ShouldNotHaveDependenciesOn_CaseSensitivity_ShouldDefaultToSensitive()
	{
		IFilterableAssemblyExpectation sut = Expect.That
			.AssemblyContaining<MockFileSystem>();

		ITestResult<IAssemblyExpectation> result =
			sut.ShouldNotHaveDependenciesOn("testably.*");

		result.IsSatisfied.Should().BeTrue();
	}

	[Fact]
	public void ShouldNotHaveDependenciesOn_ErrorsShouldIncludeNameOfAllFailedReferences()
	{
		Assembly testAssembly = typeof(MockFileSystem).Assembly;
		string[] expectedReferences = testAssembly.GetReferencedAssemblies()
			.Select(x => x.FullName)
			.ToArray();
		IFilterableAssemblyExpectation sut = Expect.That.Assemblies(testAssembly);

		ITestResult<IAssemblyExpectation> result =
			sut.ShouldNotHaveDependenciesOn("*");

		result.IsSatisfied.Should().BeFalse();

		result.Errors.Length.Should().Be(1);
		TestError error = result.Errors.Single();
		error.Should().BeOfType<DependencyTestError>();
		DependencyTestError dependencyTestError = (DependencyTestError)error;

		dependencyTestError.Assembly.Should().BeSameAs(testAssembly);
		dependencyTestError.AssemblyReferences.Length.Should()
			.Be(expectedReferences.Length);
		foreach (string reference in expectedReferences)
		{
			dependencyTestError.AssemblyReferences
				.Select(x => x.FullName)
				.Should().Contain(reference);
		}
	}

	[Fact]
	public void ShouldNotHaveDependenciesOn_WithDependencyStartingWithPrefix_ShouldReturnFalse()
	{
		IFilterableAssemblyExpectation sut = Expect.That
			.AssemblyContaining<MockFileSystem>();

		ITestResult<IAssemblyExpectation> result =
			sut.ShouldNotHaveDependenciesOn("Testably.*");

		result.IsSatisfied.Should().BeFalse();
	}

	[Theory]
	[InlineData(true, false)]
	[InlineData(false, true)]
	public void ShouldNotHaveDependenciesOn_WithIgnoreCaseParameter_ShouldConsiderCaseSensitivity(
		bool ignoreCase, bool expectedResult)
	{
		IFilterableAssemblyExpectation sut = Expect.That
			.AssemblyContaining<MockFileSystem>();

		ITestResult<IAssemblyExpectation> result =
			sut.ShouldNotHaveDependenciesOn("testably.*", ignoreCase);

		result.IsSatisfied.Should().Be(expectedResult);
	}
}
