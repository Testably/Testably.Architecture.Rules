using AutoFixture.Xunit2;
using FluentAssertions;
using System.Linq;
using System.Reflection;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Internal;

public sealed class EventRuleTests
{
	[Fact]
	public void ShouldSatisfy_DefaultError_ShouldIncludeEventInfoName()
	{
		EventInfo eventInfo = typeof(DummyClass).GetEvents().First();
		string expectedEventInfoName = $"'{eventInfo.Name}'";
		IRule rule = Expect.That.Events
			.Which(t => t == eventInfo)
			.ShouldSatisfy(_ => false);

		ITestResult result = rule.Check
			.In(typeof(DummyClass).Assembly);

		TestError error = result.Errors.Single();
		error.ToString().Should().Contain(expectedEventInfoName);
	}

	[Theory]
	[AutoData]
	public void ShouldSatisfy_False_ShouldIncludeError(TestError error)
	{
		EventInfo eventInfo = typeof(DummyClass).GetEvents().First();
		IRule rule = Expect.That.Events
			.Which(t => t == eventInfo)
			.ShouldSatisfy(Requirement.ForEvent(_ => false, _ => error));

		ITestResult result = rule.Check
			.In(typeof(DummyClass).Assembly);

		result.Errors.Should().NotBeEmpty();
		result.Errors.Single().Should().Be(error);
	}

	[Theory]
	[AutoData]
	public void ShouldSatisfy_True_ShouldNotIncludeError(TestError error)
	{
		EventInfo eventInfo = typeof(DummyClass).GetEvents().First();
		IRule rule = Expect.That.Events
			.Which(t => t == eventInfo)
			.ShouldSatisfy(Requirement.ForEvent(_ => true, _ => error));

		ITestResult result = rule.Check
			.In(typeof(DummyClass).Assembly);

		result.Errors.Should().BeEmpty();
	}

	[Fact]
	public void Which_ShouldFilterOutEventInfos()
	{
		int allEventsCount = typeof(DummyClass).GetEvents().Length;

		IRule rule = Expect.That.Events
			.Which(t => t.DeclaringType == typeof(DummyClass)).And
			.Which(p => p.Name.StartsWith(nameof(DummyClass.DummyEvent1)))
			.ShouldSatisfy(Requirement.ForEvent(_ => false));

		ITestResult result = rule.Check
			.In(typeof(DummyClass).Assembly);

		result.Errors.Length.Should().BeLessThan(allEventsCount);
		result.Errors.Should()
			.OnlyContain(e => e.ToString().Contains($"'{nameof(DummyClass.DummyEvent1)}"));
	}
}
