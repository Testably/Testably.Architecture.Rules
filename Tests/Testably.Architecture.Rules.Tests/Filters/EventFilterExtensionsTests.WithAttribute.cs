using FluentAssertions;
using System;
using System.Reflection;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

// ReSharper disable ClassNeverInstantiated.Local
// ReSharper disable EventNeverSubscribedTo.Local
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

		[Fact]
		public void ShouldSatisfy_ShouldApplyOnFilteredResult()
		{
			IEventFilterResult sut = Have.Event
				.WithAttribute<FooAttribute>().OrAttribute<BarAttribute>();

			IRequirementResult<EventInfo> rule = sut.ShouldSatisfy(_ => false);

			ITestResult result = rule.Check.InAllLoadedAssemblies();
			result.Errors.Length.Should().Be(2);
			result.Errors.Should().Contain(e
				=> ((EventTestError)e).Event.DeclaringType == typeof(FooClass));
			result.Errors.Should().Contain(e
				=> ((EventTestError)e).Event.DeclaringType == typeof(BarClass));
		}

		[Fact]
		public void Types_ShouldForwardToFilteredResult()
		{
			IEventFilterResult sut = Have.Event
				.WithAttribute<FooAttribute>().OrAttribute<BarAttribute>();

			IRequirementResult<Type> rule = sut.Types.ShouldSatisfy(_ => false);

			ITestResult result = rule.Check.InAllLoadedAssemblies();
			result.Errors.Length.Should().Be(2);
			result.Errors.Should().Contain(e
				=> ((TypeTestError)e).Type == typeof(FooClass));
			result.Errors.Should().Contain(e
				=> ((TypeTestError)e).Type == typeof(BarClass));
		}

		[AttributeUsage(AttributeTargets.Event)]
		private class BarAttribute : Attribute
		{
		}

		public delegate void Dummy();
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
