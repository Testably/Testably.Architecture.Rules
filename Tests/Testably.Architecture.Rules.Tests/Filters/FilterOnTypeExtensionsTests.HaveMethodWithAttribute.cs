using FluentAssertions;
using System;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

public class FilterOnTypeWithMethodAttributeTests
{
	[Fact]
	public void OrAttribute_ShouldReturnBothTypes()
	{
		ITestResult result = Expect.That.Types
			.WhichHaveMethodWithAttribute<FooAttribute>()
			.OrAttribute<BarAttribute>()
			.GetMatchingTypesAsErrorInAllLoadedAssemblies();

		result.Errors.Length.Should().Be(2);
	}

	[Fact]
	public void WhichHaveMethodWithAttribute_ShouldReturnMatchingType()
	{
		ITestResult result = Expect.That.Types
			.WhichHaveMethodWithAttribute<FooAttribute>()
			.GetMatchingTypesAsErrorInAllLoadedAssemblies();

		result.Errors.Length.Should().Be(1);
		result.Errors[0].ToString().Should().Contain(nameof(FooClass));
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
