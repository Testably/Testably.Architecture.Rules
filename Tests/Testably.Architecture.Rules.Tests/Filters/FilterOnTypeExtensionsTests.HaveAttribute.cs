using FluentAssertions;
using System;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

public class FilterOnTypeWithAttributeTests
{
	[Fact]
	public void OrAttribute_ShouldReturnBothTypes()
	{
		ITestResult result = Expect.That.Types
			.WhichHaveAttribute<FooAttribute>()
			.OrAttribute<BarAttribute>()
			.GetMatchingTypesAsErrorInAllLoadedAssemblies();

		result.Errors.Length.Should().Be(2);
	}

	[AttributeUsage(AttributeTargets.Class)]
	private class BarAttribute : Attribute
	{
	}

	[Bar]
	// ReSharper disable once UnusedType.Local
	private class BarClass
	{
	}

	[AttributeUsage(AttributeTargets.Class)]
	private class FooAttribute : Attribute
	{
	}

	[Foo]
	// ReSharper disable once UnusedType.Local
	private class FooClass
	{
	}
}
