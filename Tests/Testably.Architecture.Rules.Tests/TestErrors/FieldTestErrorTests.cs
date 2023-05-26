using AutoFixture.Xunit2;
using FluentAssertions;
using System.Linq;
using System.Reflection;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.TestErrors;

public sealed class FieldTestErrorTests
{
	[Fact]
	public void Field_ShouldSetField()
	{
		FieldInfo fieldInfo = typeof(DummyClass).GetFields().First();

		FieldTestError sut = new(fieldInfo, "foo");

		sut.Field.Should().BeSameAs(fieldInfo);
	}

	[Theory]
	[AutoData]
	public void ToString_ShouldReturnMessage(string message)
	{
		FieldInfo fieldInfo = typeof(DummyClass).GetFields().First();

		FieldTestError sut = new(fieldInfo, message);
		string result = sut.ToString();

		result.Should().Be(message);
	}
}
