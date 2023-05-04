using FluentAssertions;
using System;
using Xunit;

namespace Testably.Architecture.Testing.Tests.Filters;

public class FilterOnTypeWithAttributeTests
{
	[Fact]
	public void OrAttribute_ShouldReturnBothTypes()
	{
		FilterOnType.WithAttribute sut = Expect.That.AllLoadedTypes()
			.WhichHaveAttribute<FooAttribute>();

		FilterOnType.WithAttribute result = sut.OrAttribute<BarAttribute>();

		result.ShouldSatisfy(_ => false).Errors.Length.Should().Be(2);
	}

	[AttributeUsage(AttributeTargets.Class)]
	private class BarAttribute : Attribute
	{
	}

	[Bar]
	private class BarClass
	{
	}

	[AttributeUsage(AttributeTargets.Class)]
	private class FooAttribute : Attribute
	{
	}

	[Foo]
	private class FooClass
	{
	}
}
