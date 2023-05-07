using FluentAssertions;
using System;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Extensions;

public sealed class TestResultExtensionsTests
{
	[Fact]
	public void ThrowIfViolated_NotViolated_ShouldReturnTestResultWithoutDescription()
	{
		ITestResult result = Expect.That.Assemblies
			.ShouldSatisfy(_ => true)
			.Check.InExecutingAssembly()
			.ThrowIfViolated();

		result.ToString().Should()
			.NotContain(
				nameof(ThrowIfViolated_NotViolated_ShouldReturnTestResultWithoutDescription));
	}

	[Fact]
	public void ThrowIfViolated_ShouldIncludeTestNameAsDescription()
	{
		Exception? exception = Record.Exception(() =>
		{
			Expect.That.Assemblies
				.ShouldAlwaysFail()
				.Check.InExecutingAssembly()
				.ThrowIfViolated();
		});

		exception.Should().NotBeNull();
		exception!.Message.Should()
			.Contain(nameof(ThrowIfViolated_ShouldIncludeTestNameAsDescription));
	}
}
