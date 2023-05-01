using FluentAssertions;
using Xunit;

namespace Testably.Architecture.Testing.Tests;

public sealed class ExtensionsForIExpectationTests
{
	[Fact]
	public void AssembliesMatching_FoundMatch_ShouldIncludeAssembly()
	{
		IFilterableAssemblyExpectation sut = Expect.That
			.AssembliesMatching("*Architecture.Testing");

		ITestResult<IAssemblyExpectation> result = sut.ShouldSatisfy(_ => false);

		result.Errors.Should().NotBeEmpty();
	}

	[Theory]
	[InlineData(false)]
	[InlineData(true)]
	public void AssembliesMatching_CaseMismatch_ShouldIncludeAssemblyWhenIgnoreCase(bool ignoreCase)
	{
		IFilterableAssemblyExpectation sut = Expect.That
			.AssembliesMatching("testably.*", ignoreCase);

		ITestResult<IAssemblyExpectation> result = sut.ShouldSatisfy(_ => false);

		(result.Errors.Length > 0).Should().Be(ignoreCase);
	}
}
