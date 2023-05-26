using FluentAssertions;
using System;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

public sealed partial class EventFilterExtensionsTests
{
	public sealed class WithAttributeTests
	{
		[Fact]
		public void OrAttribute_ShouldReturnBothTypes()
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(BarClass), typeof(FooClass))
				.Should(Have.Event.WithAttribute<FooAttribute>().OrAttribute<BarAttribute>())
				.Check.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		[Fact]
		public void OrAttribute_ShouldUseCorrectErrorMessage()
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(BarClass))
				.Should(Have.Event.WithAttribute<FooAttribute>())
				.Check.InAllLoadedAssemblies();

			result.Errors.Length.Should().Be(1);
			result.Errors[0].ToString().Should()
				.Contain($"type '{typeof(BarClass)}'")
				.And.Contain("event")
				.And.Contain(nameof(FooAttribute));
		}

		[AttributeUsage(AttributeTargets.Event)]
		private class BarAttribute : Attribute
		{
		}

		public delegate void Dummy(); // ReSharper disable ClassNeverInstantiated.Local
		// ReSharper disable EventNeverSubscribedTo.Local
		#pragma warning disable CS8618
		#pragma warning disable CS0067
		private class BarClass
		{
			[Bar]
			public event Dummy BarEvent;
		}

		[AttributeUsage(AttributeTargets.Event)]
		private class FooAttribute : Attribute
		{
		}

		private class FooClass
		{
			[Foo]
			public event Dummy FooEvent;
		}
		#pragma warning restore CS0067
		#pragma warning restore CS8618
	}
}
