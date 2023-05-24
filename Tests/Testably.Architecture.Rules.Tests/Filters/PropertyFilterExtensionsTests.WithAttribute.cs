using FluentAssertions;
using System;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

public sealed partial class PropertyFilterExtensionsTests
{
	public sealed class WithAttributeTests
	{
		[Fact]
		public void OrAttribute_ShouldReturnBothTypes()
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(BarClass), typeof(FooClass))
				.Should(Have.Property.WithAttribute<FooAttribute>().OrAttribute<BarAttribute>())
				.Check.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		[Fact]
		public void OrAttribute_ShouldUseCorrectErrorMessage()
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(BarClass))
				.Should(Have.Property.WithAttribute<FooAttribute>())
				.Check.InAllLoadedAssemblies();

			result.Errors.Length.Should().Be(1);
			result.Errors[0].ToString().Should()
				.Contain($"properties of type '{typeof(BarClass)}'");
		}

		[AttributeUsage(AttributeTargets.Property)]
		private class BarAttribute : Attribute
		{
		} // ReSharper disable ClassNeverInstantiated.Local
		// ReSharper disable UnusedMember.Local
		private class BarClass
		{
			[Bar]
			public int BarProperty { get; set; }
		}

		[AttributeUsage(AttributeTargets.Property)]
		private class FooAttribute : Attribute
		{
		}

		private class FooClass
		{
			[Foo]
			public int FooProperty { get; set; }
		}
	}
}
