using AutoFixture.Xunit2;
using FluentAssertions;
using System.Linq;
using System.Reflection;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.TestErrors;

public sealed class ConstructorTestErrorTests
{
	[Fact]
	public void Constructor_ShouldSetConstructor()
	{
		ConstructorInfo constructorInfo = typeof(DummyClass).GetConstructors().First();

		ConstructorTestError sut = new(constructorInfo, "foo");

		sut.Constructor.Should().BeSameAs(constructorInfo);
	}

	[Theory]
	[AutoData]
	public void ToString_ShouldReturnMessage(string message)
	{
		ConstructorInfo constructorInfo = typeof(DummyClass).GetConstructors().First();

		ConstructorTestError sut = new(constructorInfo, message);
		string result = sut.ToString();

		result.Should().Be(message);
	}
}
