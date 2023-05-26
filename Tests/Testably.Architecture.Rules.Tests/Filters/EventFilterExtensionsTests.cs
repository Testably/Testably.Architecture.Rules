using AutoFixture.Xunit2;
using FluentAssertions;
using System.Linq;
using System.Reflection;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

public sealed partial class EventFilterExtensionsTests
{
	[Theory]
	[InlineData(false)]
	[InlineData(true)]
	public void Which_WithExpression_ShouldConsiderPredicateResult(bool predicateResult)
	{
		EventInfo eventInfo = typeof(DummyClass).GetEvents().First();
		IEventFilterResult sut = Have.Event
			.Which(_ => predicateResult);

		bool result = sut.Applies(eventInfo);

		result.Should().Be(predicateResult);
	}

	[Theory]
	[InlineAutoData(false)]
	[InlineAutoData(true)]
	public void Which_WithName_ShouldConsiderPredicateResult(bool predicateResult, string name)
	{
		EventInfo eventInfo = typeof(DummyClass).GetEvents().First();
		IEventFilterResult sut = Have.Event
			.Which(_ => predicateResult, name);

		bool result = sut.Applies(eventInfo);

		result.Should().Be(predicateResult);
	}
}
