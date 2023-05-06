using Testably.Abstractions.Testing;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Exemptions;

public sealed class ExemptionOnAssemblyExtensionsTests
{
	[Fact]
	public void ExceptDependencyOn_WhenFilteringAllErrors_ShouldReturnSuccess()
	{
		IRule rule = Expect.That.Assemblies
			.ShouldNotHaveDependenciesOn("Testably.*")
			.ExceptDependencyOn("Testably.Abstractions.Interface");

		rule.Check
			.InAssemblyContaining<MockFileSystem>()
			.ShouldNotBeViolated();
	}

	[Theory]
	[InlineData(false)]
	[InlineData(true)]
	public void ExceptDependencyOn_WithIgnoreCaseParameter_ShouldConsiderCaseSensitivity(
		bool ignoreCase)
	{
		IRule rule = Expect.That.Assemblies
			.ShouldNotHaveDependenciesOn("Testably.*")
			.ExceptDependencyOn("testably.ABSTRACTIONS.interface", ignoreCase);

		rule.Check
			.InAssemblyContaining<MockFileSystem>()
			.ShouldBeViolatedIf(!ignoreCase);
	}
}
