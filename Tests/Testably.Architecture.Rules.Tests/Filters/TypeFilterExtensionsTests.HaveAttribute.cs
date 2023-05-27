using FluentAssertions;
using System;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

// ReSharper disable UnusedType.Local
public sealed partial class TypeFilterExtensionsTests
{
	public sealed class HaveAttributeTests
	{
		[Fact]
		public void OrAttribute_ShouldReturnBothTypes()
		{
			ITypeFilter source = Expect.That.Types;

			ITypeFilterResult sut = source.WhichHaveAttribute<FooAttribute>()
				.OrAttribute<BarAttribute>();

			ITestResult result = sut
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();
			sut.ToString().Should().Contain(
				$"has attribute {nameof(FooAttribute)} or has attribute {nameof(BarAttribute)}");
			result.Errors.Length.Should().Be(2);
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
}
