using FluentAssertions;
using System.Reflection;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Requirements;

public sealed partial class RequirementOnEventExtensionsTests
{
	public sealed class MatchNameTests
	{
		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void ShouldMatchName_IgnoreCase_ShouldConsiderParameter(bool ignoreCase)
		{
			EventInfo @event =
				typeof(DummyFooClass).GetEvent(nameof(DummyFooClass.DummyFooEvent1))!;
			IRule rule = Expect.That.Events
				.WhichAre(@event)
				.ShouldMatchName("DUMMYfooEVENT1", ignoreCase);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(!ignoreCase);
		}

		[Theory]
		[InlineData("*")]
		[InlineData("?????FooEvent1")]
		[InlineData("DummyFooEvent1")]
		public void ShouldMatchName_MatchingPattern_ShouldNotBeViolated(string matchingPattern)
		{
			EventInfo @event =
				typeof(DummyFooClass).GetEvent(nameof(DummyFooClass.DummyFooEvent1))!;
			IRule rule = Expect.That.Events
				.WhichAre(@event)
				.ShouldMatchName(matchingPattern);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		[Theory]
		[InlineData("*Foo")]
		[InlineData("??FooEvent1")]
		[InlineData("dummyfooevent1")]
		public void ShouldMatchName_NotMatchingPattern_ShouldNotBeSatisfied(
			string notMatchingPattern)
		{
			EventInfo @event =
				typeof(DummyFooClass).GetEvent(nameof(DummyFooClass.DummyFooEvent1))!;
			IRule rule = Expect.That.Events
				.WhichAre(@event)
				.ShouldMatchName(notMatchingPattern);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolated();
			result.Errors[0].Should().BeOfType<EventTestError>()
				.Which.Event.Should().BeSameAs(@event);
			result.Errors[0].Should().BeOfType<EventTestError>()
				.Which.ToString().Should().Contain(@event.Name).And.Contain($"'{notMatchingPattern}'");
		}

		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void ShouldNotMatchName_IgnoreCase_ShouldConsiderParameter(bool ignoreCase)
		{
			EventInfo @event =
				typeof(DummyFooClass).GetEvent(nameof(DummyFooClass.DummyFooEvent1))!;
			IRule rule = Expect.That.Events
				.WhichAre(@event)
				.ShouldNotMatchName("DUMMYfooEVENT1", ignoreCase);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(ignoreCase);
		}

		[Theory]
		[InlineData("*")]
		[InlineData("?????FooEvent1")]
		[InlineData("DummyFooEvent1")]
		public void ShouldNotMatchName_MatchingPattern_ShouldNotBeSatisfied(string matchingPattern)
		{
			EventInfo @event =
				typeof(DummyFooClass).GetEvent(nameof(DummyFooClass.DummyFooEvent1))!;
			IRule rule = Expect.That.Events
				.WhichAre(@event)
				.ShouldNotMatchName(matchingPattern);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolated();
			result.Errors[0].Should().BeOfType<EventTestError>()
				.Which.Event.Should().BeSameAs(@event);
			result.Errors[0].Should().BeOfType<EventTestError>()
				.Which.ToString().Should().Contain(@event.Name).And.Contain($"'{matchingPattern}'");
		}

		[Theory]
		[InlineData("*Foo")]
		[InlineData("??FooEvent1")]
		[InlineData("dummyfooevent1")]
		public void ShouldNotMatchName_NotMatchingPattern_ShouldNotBeViolated(
			string notMatchingPattern)
		{
			EventInfo @event =
				typeof(DummyFooClass).GetEvent(nameof(DummyFooClass.DummyFooEvent1))!;
			IRule rule = Expect.That.Events
				.WhichAre(@event)
				.ShouldNotMatchName(notMatchingPattern);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}
	}
}
