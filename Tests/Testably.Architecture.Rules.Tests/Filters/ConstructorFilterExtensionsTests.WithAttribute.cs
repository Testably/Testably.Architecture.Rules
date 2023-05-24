using FluentAssertions;
using System;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

public sealed partial class ConstructorFilterExtensionsTests
{
	public sealed class WithAttributeTests
	{
		[Fact]
		public void OrAttribute_ShouldReturnBothTypes()
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(BarClass), typeof(FooClass))
				.Should(Have.Constructor.WithAttribute<FooAttribute>().OrAttribute<BarAttribute>())
				.Check.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		[Fact]
		public void OrAttribute_ShouldUseCorrectErrorMessage()
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(BarClass))
				.Should(Have.Constructor.WithAttribute<FooAttribute>())
				.Check.InAllLoadedAssemblies();

			result.Errors.Length.Should().Be(1);
			result.Errors[0].ToString().Should()
				.Contain($"constructors of type '{typeof(BarClass)}'");
		}

		[AttributeUsage(AttributeTargets.Constructor)]
		private class BarAttribute : Attribute
		{
		}

		// ReSharper disable once ClassNeverInstantiated.Local
		private class BarClass
		{
			[Bar]
			// ReSharper disable once UnusedMember.Local
			public BarClass()
			{
			}
		}

		[AttributeUsage(AttributeTargets.Constructor)]
		private class FooAttribute : Attribute
		{
		}

		// ReSharper disable once ClassNeverInstantiated.Local
		private class FooClass
		{
			[Foo]
			// ReSharper disable once UnusedMember.Local
			public FooClass()
			{
			}
		}
	}
}
