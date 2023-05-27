using AutoFixture.Xunit2;
using FluentAssertions;
using System.Linq;
using System.Reflection;
using Testably.Architecture.Rules.Internal;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Internal;

public sealed class EventRuleTests
{
	[Theory]
	[InlineData(true, true, true)]
	[InlineData(true, false, false)]
	[InlineData(false, true, false)]
	[InlineData(false, false, false)]
	public void Applies_ShouldApplyAllFilters(bool result1, bool result2, bool expectedResult)
	{
		EventInfo element = typeof(DummyFooClass).GetEvents().First();

		EventRule sut = new(
			Filter.FromPredicate<EventInfo>(_ => result1),
			Filter.FromPredicate<EventInfo>(_ => result2));

		bool result = sut.Applies(element);

		result.Should().Be(expectedResult);
	}

	[Fact]
	public void ShouldSatisfy_DefaultError_ShouldIncludeEventInfoName()
	{
		EventInfo eventInfo = typeof(DummyFooClass).GetEvents().First();
		string expectedEventInfoName = $"'{eventInfo.Name}'";
		IRule rule = Expect.That.Events
			.Which(t => t == eventInfo)
			.ShouldSatisfy(_ => false);

		ITestResult result = rule.Check
			.In(typeof(DummyFooClass).Assembly);

		TestError error = result.Errors.Single();
		error.ToString().Should().Contain(expectedEventInfoName);
	}

	[Theory]
	[AutoData]
	public void ShouldSatisfy_False_ShouldIncludeError(TestError error)
	{
		EventInfo eventInfo = typeof(DummyFooClass).GetEvents().First();
		IRule rule = Expect.That.Events
			.Which(t => t == eventInfo)
			.ShouldSatisfy(Requirement.ForEvent(_ => false, _ => error));

		ITestResult result = rule.Check
			.In(typeof(DummyFooClass).Assembly);

		result.Errors.Should().NotBeEmpty();
		result.Errors.Single().Should().Be(error);
	}

	[Theory]
	[AutoData]
	public void ShouldSatisfy_True_ShouldNotIncludeError(TestError error)
	{
		EventInfo eventInfo = typeof(DummyFooClass).GetEvents().First();
		IRule rule = Expect.That.Events
			.Which(t => t == eventInfo)
			.ShouldSatisfy(Requirement.ForEvent(_ => true, _ => error));

		ITestResult result = rule.Check
			.In(typeof(DummyFooClass).Assembly);

		result.Errors.Should().BeEmpty();
	}

	[Theory]
	[AutoData]
	public void ToString_ShouldCombineFilters(string filter1, string filter2)
	{
		IRule rule = Expect.That.Events
			.Which(_ => true, filter1).And
			.Which(_ => true, filter2)
			.ShouldSatisfy(_ => true);

		rule.ToString().Should().Be($"{filter1} and {filter2}");
	}

	[Theory]
	[AutoData]
	public void Types_ShouldApplyEventFilter(string filterName)
	{
		EventInfo origin = typeof(DummyFooClass).GetEvents().First();

		IRule rule = Expect.That.Events
			.Which(c => c == origin, filterName)
			.Types
			.Which(_ => false)
			.ShouldAlwaysFail();

		ITestResult result = rule.Check
			.In(typeof(DummyFooClass).Assembly);

		result.Errors.Length.Should().Be(1);
		result.Errors[0].ToString().Should()
			.Contain(filterName).And.Contain("type must have an event");
	}

	[Fact]
	public void Types_ShouldRequireAllEvents()
	{
		EventInfo event1 = typeof(DummyFooClass).GetEvents().First();
		EventInfo event2 = typeof(DummyFooClass).GetEvents().Last();

		IRule rule = Expect.That.Events
			.Which(p => p == event1).And
			.Which(p => p == event2)
			.Types
			.ShouldAlwaysFail()
			.AllowEmpty();

		ITestResult result = rule.Check
			.In(typeof(DummyFooClass).Assembly);

		result.ShouldNotBeViolated();
	}

	[Fact]
	public void Which_ShouldFilterOutEventInfos()
	{
		int allEventsCount = typeof(DummyFooClass).GetEvents().Length;

		IRule rule = Expect.That.Events
			.Which(t => t.DeclaringType == typeof(DummyFooClass)).And
			.Which(p => p.Name.StartsWith(nameof(DummyFooClass.DummyFooEvent1)))
			.ShouldSatisfy(Requirement.ForEvent(_ => false));

		ITestResult result = rule.Check
			.In(typeof(DummyFooClass).Assembly);

		result.Errors.Length.Should().BeLessThan(allEventsCount);
		result.Errors.Should()
			.OnlyContain(e => e.ToString().Contains($"'{nameof(DummyFooClass.DummyFooEvent1)}"));
	}
}
