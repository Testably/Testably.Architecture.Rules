using FluentAssertions;
using System;
using Xunit;

namespace Testably.Architecture.Testing.Tests.Filters;

public class FilterOnTypeWithMethodAttributeTests
{
	[Fact]
	public void OrAttribute_ShouldReturnBothTypes()
	{
		FilterOnType.WithMethodAttribute sut = Expect.That.AllLoadedTypes()
			.WhichHaveMethodWithAttribute<FooAttribute>();

		FilterOnType.WithMethodAttribute result = sut.OrAttribute<BarAttribute>();

		result.ShouldSatisfy(_ => false).Errors.Length.Should().Be(2);
	}

	[AttributeUsage(AttributeTargets.Method)]
	private class BarAttribute : Attribute
	{
	}

	// ReSharper disable once UnusedType.Local
	private class BarClass
	{
		[Bar]
		// ReSharper disable once UnusedMember.Local
		public void Bar()
		{
			throw new NotSupportedException();
		}
	}

	[AttributeUsage(AttributeTargets.Method)]
	private class FooAttribute : Attribute
	{
	}

	// ReSharper disable once UnusedType.Local
	private class FooClass
	{
		[Foo]
		// ReSharper disable once UnusedMember.Local
		public void Foo()
		{
			throw new NotSupportedException();
		}
	}
}
