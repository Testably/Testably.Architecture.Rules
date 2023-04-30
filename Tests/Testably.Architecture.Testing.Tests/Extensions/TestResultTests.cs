﻿using FluentAssertions;
using Testably.Abstractions.Testing;
using Xunit;

namespace Testably.Architecture.Testing.Tests.Extensions;

public class TestResultTests
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
}
