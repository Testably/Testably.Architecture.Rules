using AutoFixture.Xunit2;
using FluentAssertions;
using System;
using Xunit;

namespace Testably.Architecture.Rules.Tests.TestErrors;

public sealed class TypeTestErrorTests
{
	[Fact]
	public void Constructor_ShouldSeTEntity()
	{
		Type type = typeof(TypeTestErrorTests);

		TypeTestError sut = new(type, "foo");

		sut.Type.Should().BeSameAs(type);
	}

	[Theory]
	[AutoData]
	public void ToString_ShouldReturnMessage(string message)
	{
		Type type = typeof(TypeTestErrorTests);

		TypeTestError sut = new(type, message);
		string result = sut.ToString();

		result.Should().Be(message);
	}
}
