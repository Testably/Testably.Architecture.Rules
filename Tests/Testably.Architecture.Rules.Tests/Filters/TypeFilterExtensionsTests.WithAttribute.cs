using FluentAssertions;
using System;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

public sealed partial class TypeFilterExtensionsTests
{
	public sealed class WithAttributeTests
	{
		[Fact]
		public void OrAttribute_ShouldReturnBothTypes()
		{
			ITypeFilter source = Expect.That.Types;

			ITypeFilterResult sut = source.WithAttribute<FooAttribute>()
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
}
