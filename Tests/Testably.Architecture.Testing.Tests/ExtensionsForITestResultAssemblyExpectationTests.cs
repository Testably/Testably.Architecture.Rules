using FluentAssertions;
using Testably.Abstractions.Testing;
using Xunit;

namespace Testably.Architecture.Testing.Tests;

public sealed class ExtensionsForITestResultAssemblyExpectationTests
{
	[Fact]
	public void ExceptDependencyOn_WhenFilteringAllErrors_ShouldReturnSuccess()
	{
		IFilterableAssemblyExpectation sut = Expect.That
			.AssemblyContaining<MockFileSystem>();

		ITestResult<IAssemblyExpectation> result =
			sut.ShouldNotHaveDependenciesOn("Testably.*")
				.ExceptDependencyOn("Testably.Abstractions.Interface");

		result.IsSatisfied.Should().BeTrue();
		result.Errors.Length.Should().Be(0);
	}

	[Theory]
	[InlineData(false)]
	[InlineData(true)]
	public void ExceptDependencyOn_WithIgnoreCaseParameter_ShouldConsiderCaseSensitivity(
		bool ignoreCase)
	{
		IFilterableAssemblyExpectation sut = Expect.That
			.AssemblyContaining<MockFileSystem>();

		ITestResult<IAssemblyExpectation> result =
			sut.ShouldNotHaveDependenciesOn("Testably.*")
				.ExceptDependencyOn("testably.Abstractions.Interface", ignoreCase);

		result.IsSatisfied.Should().Be(ignoreCase);
	}
}
