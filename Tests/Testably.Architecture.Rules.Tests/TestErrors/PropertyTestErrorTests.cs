using AutoFixture.Xunit2;
using FluentAssertions;
using System.Linq;
using System.Reflection;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.TestErrors;

public sealed class PropertyTestErrorTests
{
	[Fact]
	public void Property_ShouldSetProperty()
	{
		PropertyInfo propertyInfo = typeof(DummyFooClass).GetProperties().First();

		PropertyTestError sut = new(propertyInfo, "foo");

		sut.Property.Should().BeSameAs(propertyInfo);
	}

	[Theory]
	[AutoData]
	public void ToString_ShouldReturnMessage(string message)
	{
		PropertyInfo propertyInfo = typeof(DummyFooClass).GetProperties().First();

		PropertyTestError sut = new(propertyInfo, message);
		string result = sut.ToString();

		result.Should().Be(message);
	}
}
