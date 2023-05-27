using AutoFixture.Xunit2;
using FluentAssertions;
using System.Linq;
using System.Reflection;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.TestErrors;

public sealed class EventTestErrorTests
{
	[Fact]
	public void Event_ShouldSetEvent()
	{
		EventInfo eventInfo = typeof(DummyFooClass).GetEvents().First();

		EventTestError sut = new(eventInfo, "foo");

		sut.Event.Should().BeSameAs(eventInfo);
	}

	[Theory]
	[AutoData]
	public void ToString_ShouldReturnMessage(string message)
	{
		EventInfo eventInfo = typeof(DummyFooClass).GetEvents().First();

		EventTestError sut = new(eventInfo, message);
		string result = sut.ToString();

		result.Should().Be(message);
	}
}
