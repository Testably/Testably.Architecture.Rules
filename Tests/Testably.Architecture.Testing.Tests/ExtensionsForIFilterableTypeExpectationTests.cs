﻿using FluentAssertions;
using Xunit;

namespace Testably.Architecture.Testing.Tests;

public sealed class ExtensionsForIFilterableTypeExpectationTests
{
	[Fact]
	public void X()
	{
		ITestResult result = Expect.That.AllLoadedTypes()
			.WhichHaveMethodWithAttribute<FactAttribute>().OrAttribute<TheoryAttribute>()
			.ShouldBeSealed();

		result.Errors.Should().BeEmpty();
	}
}
