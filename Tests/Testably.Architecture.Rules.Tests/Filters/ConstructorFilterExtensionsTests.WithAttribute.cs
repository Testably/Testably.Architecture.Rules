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
			string errorString = result.Errors[0].ToString();
			errorString.Should()
				.Contain($"type '{typeof(BarClass)}'")
				.And.Contain("constructor")
				.And.Contain(nameof(FooAttribute));
		}

		// ReSharper disable ClassNeverInstantiated.Local
		// ReSharper disable UnusedMember.Local
		// ReSharper disable UnusedParameter.Local

		[AttributeUsage(AttributeTargets.Constructor)]
		private class BarAttribute : Attribute
		{
		}

		private class BarClass
		{
			[Bar]
			public BarClass(int value)
			{
			}
		}

		[AttributeUsage(AttributeTargets.Constructor)]
		private class FooAttribute : Attribute
		{
		}

		private class FooClass
		{
			[Foo]
			public FooClass(int value)
			{
			}
		}
	}
}
