using FluentAssertions;
using Testably.Abstractions.Testing;
using Xunit;

namespace Testably.Architecture.Testing.Tests;

public sealed class ExtensionsForExpectationResultOnAssemblyTests
{
	[Fact]
	public void ExceptDependencyOn_WhenFilteringAllErrors_ShouldReturnSuccess()
	{
		IAssemblyExpectation sut = Expect.That
			.AssemblyContaining<MockFileSystem>();

		ITestResult result =
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
		IAssemblyExpectation sut = Expect.That
			.AssemblyContaining<MockFileSystem>();

		ITestResult result =
			sut.ShouldNotHaveDependenciesOn("Testably.*")
				.ExceptDependencyOn("testably.Abstractions.Interface", ignoreCase);

		result.IsSatisfied.Should().Be(ignoreCase);
	}
}
