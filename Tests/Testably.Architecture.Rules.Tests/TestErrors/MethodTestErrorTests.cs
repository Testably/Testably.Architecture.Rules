using AutoFixture.Xunit2;
using FluentAssertions;
using System.Linq;
using System.Reflection;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.TestErrors;

public sealed class MethodTestErrorTests
{
	[Fact]
	public void Method_ShouldSetMethod()
	{
		MethodInfo methodInfo = typeof(DummyFooClass).GetDeclaredMethods().First();

		MethodTestError sut = new(methodInfo, "foo");

		sut.Method.Should().BeSameAs(methodInfo);
	}

	[Theory]
	[AutoData]
	public void ToString_ShouldReturnMessage(string message)
	{
		MethodInfo methodInfo = typeof(DummyFooClass).GetDeclaredMethods().First();

		MethodTestError sut = new(methodInfo, message);
		string result = sut.ToString();

		result.Should().Be(message);
	}
}
